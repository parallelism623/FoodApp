using FoodShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodShop.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FoodShop.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Domain.Entities.Identity.AppUser, AppRole, Guid>
    {
        private readonly IHttpContextAccessor _context;
        public ApplicationDbContext(IHttpContextAccessor context, DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _context = context;
        }
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

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        //                                           CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var userName = (_context.HttpContext.User.Identity as ClaimsPrincipal)?.FindFirst(x => x.Type.Equals(ClaimTypes.Name));
        //    var AddedEntities = ChangeTracker.Entries()
        //        .Where(E => E.State == EntityState.Added)
        //        .ToList();

        //    AddedEntities.ForEach(E =>
        //    {
        //        E.Property("CreateBy").CurrentValue = userName;  
        //        E.Property("CreateDate").CurrentValue = DateTime.Now;
        //    });

        //    var EditedEntities = ChangeTracker.Entries()
        //        .Where(E => E.State == EntityState.Modified)
        //        .ToList();

        //    EditedEntities.ForEach(E =>
        //    {
        //        E.Property("UpdateBy").CurrentValue = userName;
        //        E.Property("UpdateDate").CurrentValue = DateTime.Now;
        //    });

        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}
    }
}
