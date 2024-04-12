using FoodShop.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Entities
{
    public class Product : DomainEntity<Guid>
    {
        
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public decimal Price { get; set; }  
        public int DiscountPercent { get; set; }
        public decimal Discount { get; set; }
        public string ImageLink { get; set; }
        public string[] ImageList { get; set; }
        public string VideoLink { get; set; }
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
        
        public int View { get; set; }
        public decimal FinalPrice => Price - Price * DiscountPercent / 100 - Discount;
        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }  
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
