using FoodShop.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Entities
{
    public class Order : DomainEntity<Guid>
    {
        public int Status { get; set; }
        public decimal Amout { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }   
        public decimal Discount { get; set; }
        public decimal AmoutTotal { get; set; }
        public Guid UserId { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
