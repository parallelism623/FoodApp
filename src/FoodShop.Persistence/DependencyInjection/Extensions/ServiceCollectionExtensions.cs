
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Domain.Abstraction;
using FoodShop.Persistence.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodShop.Persistence.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlConfiguration(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                builder.EnableDetailedErrors(true)
                       .EnableSensitiveDataLogging(true)
                       .UseLazyLoadingProxies(true)
                       .UseSqlServer(
                            connectionString: configuration.GetConnectionString("DefaultConnection"),
                            sqlServerOptionsAction: builder => builder.MigrationsAssembly(AssemblyReference.Assembly.GetName().Name)
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                       );
            }); 
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true; //default true
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // default 5
                options.Lockout.MaxFailedAccessAttempts = 3; //default 5

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.SignIn.RequireConfirmedEmail = true;
            });
        }
        public static void AddRepositoriesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ICommandRepository, CommandRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
