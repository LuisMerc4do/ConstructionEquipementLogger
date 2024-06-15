using ConstructionEquipementLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ConstructionEquipementLogger.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public ConstructionEquipementModel ConstructionEquipement { get; set; }

        public DeleteModel(IConfiguration configuration)
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
                tableCmd.CommandText = $"DELETE from forklift WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}

