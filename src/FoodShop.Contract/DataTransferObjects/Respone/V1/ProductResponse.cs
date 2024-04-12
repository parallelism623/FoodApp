using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.DataTransferObjects.Response.V1
{
    public class ProductResponse : ProductResponseList
    {
        public string[] ImageList { get; set; }
        public string VideoLink { get; set; }
        public int Quantity { get; set; }
        public string Code { get; set; }
    }
    public class ProductResponseList
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DiscountPercent { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice => Price - Price * DiscountPercent / 100 - Discount;
        public string ImageLink { get; set; }
    }
}
