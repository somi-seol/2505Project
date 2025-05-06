using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyApp.Namespace
{
    public class ResultsModel : PageModel
    {
        [BindProperty]
        public List<NameItems> ?OrderedItemsList {get;set;}
        public void OnGet()
        {
            OrderedItemsList = new List<NameItems>();
            using (var connection = new SqliteConnection("Data Source=foodpantry.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT StudentName, OrderedItems FROM Orders";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderedItemsList.Add(new Item
                        {
                            StudentName = reader.GetString(0),
                            OrderedItems = reader.GetString(1),
                        });
                    }
                }
            }
        }
    }
}

public class NameItems
    {
        public string StudentName { get; set; }
        public string OrderedItems { get; set; }
    }