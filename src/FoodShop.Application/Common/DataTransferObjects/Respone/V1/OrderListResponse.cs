using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Respone.V1
{
    public class OrderListResponse
    {
        public int Status { get; set; }
        public decimal Amout { get; set; }
        public decimal Discount { get; set; }
        public decimal AmoutTotal { get; set; }
        public Guid UserId { get; set; }
    }
}
