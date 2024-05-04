using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Extensions
{
    public class OrderExtensions
    {
        public static string GetSortOrderProperty(string sortColumn)
            => sortColumn.ToLower() switch
            {
                "amout" => "Amout",
                "tax" => "Tax",
                "discount" => "Discount",
                "amouttotal" => "AmoutTotal",
                _ => "Id"
            };
    }
}
