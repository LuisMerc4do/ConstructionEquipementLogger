using ConstructionEquipementLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConstructionEquipementLogger.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public ConstructionEquipementModel ConstructionEquipement { get; set; }

        public UpdateModel(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        public IActionResult OnGet(int id)
        {
            ConstructionEquipement = GetById(id);
            return Page();
        }
        private ConstructionEquipementModel GetById(int id)
        {
            var constructionEquipementRecord = new ConstructionEquipementModel();
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM forklift WHERE Id = {id}";

                SqliteDataReader reader = tableCmd.ExecuteReader();
                while (reader.Read())
                {

                    constructionEquipementRecord.Id = reader.GetInt32(0);
                    constructionEquipementRecord.Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    constructionEquipementRecord.Quantity = reader.GetInt32(2);
                }
            }
            return constructionEquipementRecord;
        }
        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE forklift SET Date = @Date, Quantity = @Quantity WHERE Id = @Id";

                tableCmd.Parameters.AddWithValue("@Date", ConstructionEquipement.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                tableCmd.Parameters.AddWithValue("@Quantity", ConstructionEquipement.Quantity);
                tableCmd.Parameters.AddWithValue("@Id", ConstructionEquipement.Id);

                tableCmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}