using FoodShop.Persistence;
using Google;
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
                        new DataSeeder().DataRoleSeeder(appContext).Wait();
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
