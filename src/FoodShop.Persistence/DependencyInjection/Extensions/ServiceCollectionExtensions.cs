
using FoodShop.Domain.Abstraction.Repositories;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.AddIdentityCore<AppUser>()
                    .AddRoles<AppRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityCore<AppUser>()
        .AddRoles<AppRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

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

            });
        }
        public static void AddRepositoriesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
