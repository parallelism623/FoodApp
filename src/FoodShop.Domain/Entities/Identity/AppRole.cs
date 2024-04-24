using Microsoft.AspNetCore.Identity;

namespace FoodShop.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public string? Description { get; set; } = null;
        public string? RoleCode { get; set; } = null;
        public DateTime? CreateDate { get; set; } = null;
        public string? CreateBy { get; set; } = null;
        public DateTime? UpdateDate { get; set; } = null;
        public string? UpdateBy { get; set; } = null;
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
        public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }

    }
}
