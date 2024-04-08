using Microsoft.AspNetCore.Identity;

namespace FoodShop.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public string RoleCode { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
        public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
