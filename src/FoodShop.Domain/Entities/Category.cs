using FoodShop.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Entities
{
    public class Category : DomainEntity<Guid>
    {
        public Guid ParentId { get; set; }
        public string? Title { get; set; }
        public string? MetaTitle { get; set; }
        public string? Slug { get; set; }
        public string? Content { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
