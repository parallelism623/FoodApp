using FoodShop.Domain.Entities.Identity;
using FoodShop.Persistence;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.API
{

    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                        new DataSeeder().DataRoleSeeder(
                            appContext, 
                            scope.ServiceProvider.GetService<UserManager<AppUser>>(),
                            scope.ServiceProvider.GetService<RoleManager<AppRole>>()).Wait();
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                }
            }
            return app;
        }
    }
    
}
