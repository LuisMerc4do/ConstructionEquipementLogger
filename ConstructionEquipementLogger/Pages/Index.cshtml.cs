using ConstructionEquipementLogger.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ConstructionEquipementLogger.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<ConstructionEquipementModel> Records { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
			Records = GetAllRecords();
			ViewData["Total"] = Records.AsEnumerable().Sum(x => x.Quantity);
		}
        private List<ConstructionEquipementModel> GetAllRecords()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
				connection.Open();
				var tableCmd = connection.CreateCommand();
				tableCmd.CommandText =
						$"SELECT * FROM forklift";
                var tableData = new List<ConstructionEquipementModel>();
				SqliteDataReader reader = tableCmd.ExecuteReader();

				while (reader.Read())
				{
					tableData.Add(new ConstructionEquipementModel()
                    {
                        Id = reader.GetInt32(0),
						Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", CultureInfo.InvariantCulture),
						Quantity = reader.GetInt32(2)
                    }); ;
                }
                return tableData;
            }

        }
    }
}
