using ConstructionEquipementLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ConstructionEquipementLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public ConstructionEquipementModel ConstructionEquipement { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = @"
                        INSERT INTO forklift (date, quantity) 
                        VALUES (@date, @quantity)";

                    tableCmd.Parameters.AddWithValue("@date", ConstructionEquipement.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    tableCmd.Parameters.AddWithValue("@quantity", ConstructionEquipement.Quantity);

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex.Message);
                // Optionally, display an error message to the user
                ModelState.AddModelError(string.Empty, "An error occurred while saving the record. Please try again.");
                return Page();
            }
        }
    }
}

