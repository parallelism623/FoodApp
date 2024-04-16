using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;


namespace FoodShop.Persistence
{
    public class DataSeeder
    {
        public async Task DataRoleSeeder(ApplicationDbContext context)
        {
            PasswordHasher<AppUser> password = new PasswordHasher<AppUser>();
            var user = new AppUser()
            {
                UserName = "admin",
                Email = "admin",
                EmailConfirmed = true,
            };
            var passwordHash = password.HashPassword(user,"123123");
            if (!context.Users.Any())
            {
                context.Users.Add(user);
                context.SaveChangesAsync();
            }
            var roles = new List<AppRole>()
            {
                new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = RoleDefine.Admin,
                    NormalizedName = RoleDefine.Admin.ToLower(),
                    Description = "Role Admin",
                    RoleCode = "admin"
                },
                new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = RoleDefine.User,
                    NormalizedName = RoleDefine.User.ToLower(),
                    Description = "Role User",
                    RoleCode = "user"
                }
            };
            if (!context.Roles.Any())
            {
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
            if(!context.UserRoles.Any())
            {
                var userRoles = new IdentityUserRole<Guid>()
                {
                    UserId = user.Id,
                    RoleId = roles[0].Id
                };
                await context.UserRoles.AddAsync(userRoles);
                await context.SaveChangesAsync();
            }
        }
    }
}
