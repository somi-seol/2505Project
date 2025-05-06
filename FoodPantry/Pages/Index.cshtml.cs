using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;

// this is my model
namespace FoodPantry.Pages
{

    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IndexModel(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public string ?StudentName {get; set;} // todo

        [BindProperty]
        public string ?StudentId {get; set;} // todo

        [BindProperty]
        public List<Item> ?ItemList {get;set;} // todo

        [BindProperty]
        public ItemSelection ItemSelection { get; set; } = new();

        public void OnGet()
        {
            LoadItemList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid){
                LoadItemList(); // Need to reload items on invalid form
                return Page();
            }
            
           // Process selected items
            if (ItemSelection.SelectedItemIds != null && ItemSelection.SelectedItemIds.Any())
            {
                ItemSelection.SelectedItems = new List<string>();
                LoadItemList(); // Load the items to get names

                using (var connection = new SqliteConnection("Data Source=foodpantry.db"))
                {
                    connection.Open();

                    foreach (var itemId in ItemSelection.SelectedItemIds)
                    {
                        var item = ItemList.FirstOrDefault(i => i.ItemId == itemId);
                        if (item != null)
                        {
                            ItemSelection.SelectedItems.Add(item.ItemName);

                            // insert each item into Orders table
                            var command = connection.CreateCommand();
                            command.CommandText = @"
                                INSERT INTO Orders (StudentName, StudentId, OrderedItems)
                                VALUES (@StudentName, @StudentId, @OrderedItems)";
                            command.Parameters.AddWithValue("@StudentName", StudentName);
                            command.Parameters.AddWithValue("@StudentId", StudentId);
                            command.Parameters.AddWithValue("@OrderedItems", item.ItemName);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
    
            return Page(); // Show the same page with selected items
        }

        private void LoadItemList()
        {
            ItemList = new List<Item>();
            using (var connection = new SqliteConnection("Data Source=foodpantry.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ItemId, ItemName, CategoryId FROM Item";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ItemList.Add(new Item
                        {
                            ItemId = reader.GetInt32(0),
                            ItemName = reader.GetString(1),
                            CategoryId = reader.GetInt32(2)
                        });
                    }
                }
            }
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
    public string StudentId {get; set;}
    public string StudentName {get; set;}
    public string OrderedItems {get; set;}
    public string OrderDateTime {get; set;}
}

public class ItemSelection
{
    public List<int> SelectedItemIds { get; set; } = new();
    public List<string> SelectedItems { get; set; } = new();
}