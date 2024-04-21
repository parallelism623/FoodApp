using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DiscountPercent { get; set; }
        public decimal Discount { get; set; }
        public string ImageLink { get; set; }
        public string[] ImageList { get; set; }
        public string VideoLink { get; set; }

        public int Quantity { get; set; }
        public int View { get; set; }
    }
}
