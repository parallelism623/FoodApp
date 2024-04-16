using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Entities.Identity
{
    public class Permission 
    {
        public Guid RoleId { get; set; }
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
        public string Description { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionFunction { get; set; }

    }
}
