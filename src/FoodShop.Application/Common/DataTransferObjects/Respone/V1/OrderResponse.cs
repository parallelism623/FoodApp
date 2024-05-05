using FoodShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Respone.V1
{
    public class OrderResponse
    {
        public string? Title { get; set; }
        public int Status { get; set; }
        public decimal Amout { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Discount { get; set; }
        public decimal AmoutTotal { get; set; }

        public List<OrderProductRespone>? Products { get; set; }
    }
    
}
