using FoodShop.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Entities
{
    public class Cart : DomainEntity<Guid>
    {
        public Guid UserId { get; set; }
        public decimal? Amout { get; set; } 
        public virtual ICollection<CartProduct> CartProducts { get; set; }
        
    }
}
