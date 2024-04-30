using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;



namespace FoodShop.Persistence
{
    public class DataSeeder
    {
        public async Task DataRoleSeeder(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager)
        {
        
            PasswordHasher<AppUser> password = new PasswordHasher<AppUser>();
            var user = new AppUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            if (!context.Users.Where(x => x.Email.Equals("admin")).Any())
            {
                var result = await userManager.CreateAsync(user, "12345678");
            }
            var roles = new List<AppRole>()
            {
                new AppRole
                {
                    
                    Name = FSRoles.Admin,
                    NormalizedName = FSRoles.Admin.ToLower(),
                    Description = "Role Admin",
                    RoleCode = "admin"
                },
                new AppRole
                {
                   
                    Name = FSRoles.Basic,
                    NormalizedName = FSRoles.Basic.ToLower(),
                    Description = "Role Basic",
                    RoleCode = "basic"
                }
            };
            if (!context.Roles.Any())
            {
                foreach(var role in roles) 
                {
                    await roleManager.CreateAsync(role);
                }
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
                {
                    RoleId = roles[0].Id,
                    UserId = user.Id,
                });
                await context.SaveChangesAsync();
            }

            if (context.RoleClaims.Any() == false)
            {
                var roleAdmin = await roleManager.FindByNameAsync("admin");
                foreach (var fr in typeof(FSResource).GetFields())
                {
                    foreach (var fa in typeof(FSAction).GetFields())
                    {
                        var newClaim = new Claim("Permission", string.Concat(fr.Name + ".", fa.Name));
                        await roleManager.AddClaimAsync(roleAdmin, newClaim);
                    
                    }
                }
            }
        }
    }
}
