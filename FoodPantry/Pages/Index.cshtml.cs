using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;

namespace FoodPantry.Pages
{

    public class IndexModel : PageModel
    {
        [BindProperty]
        public IndexModel Index {get; set;}

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            return Page();
        }
    

        private Item GetItemById(int ItemId)
        {
            using (var connection = new SqliteConnection("Data Source=foodpantry.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Items WHERE ItemId = @ItemId";
                command.Parameters.AddWithValue("@ItemId", ItemId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Item
                        {
                            ItemId = reader.GetInt32(0),
                            ItemName = reader.GetString(1),
                        };
                    }
                }
            }
            return null;
        }
    }
}

public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

public class Item
    {
        public int ItemId {get; set; }
        public string ItemName {get; set;}
        public int CategoryId {get; set;}
    }

public class Orders
    {
        public int OrderId {get; set;}
        public int StudentId {get; set;}
        public string StudentName {get; set;}
        public string OrderedItems {get; set;}
        public string OrderDateTime {get; set;}
    }