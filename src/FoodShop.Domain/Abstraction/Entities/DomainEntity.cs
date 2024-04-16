using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Abstraction.Entities
{
    public abstract class DomainEntity<T>
    {
        public virtual T Id { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual string UpdateBy { get; set; }
    }
}
