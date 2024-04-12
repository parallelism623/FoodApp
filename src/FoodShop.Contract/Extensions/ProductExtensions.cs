using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Extensions
{
    public class ProductExtensions
    {
        public static string GetSortProductProperty(string? sortProductProperty)
            => sortProductProperty?.ToLower() switch
            {
                "name" => "Name",
                "price" => "Price",
                "description" => "Description",
                _ => "Id"
            };
    }
}
