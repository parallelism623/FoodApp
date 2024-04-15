using FoodShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodShop.Domain.Entities;


namespace FoodShop.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Domain.Entities.Identity.AppUser, AppRole, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        }

        public DbSet<Domain.Entities.Identity.AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderDetails { get; set; }
        public DbSet<CartProduct> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        
    }
}
