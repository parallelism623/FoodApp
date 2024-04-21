using FoodShop.Application.Common.Dapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Application.Common.Repositories.Queries;
using FoodShop.Infrastructure.Dapper.Repositories.Query;
using FoodShop.Persistence.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;

namespace FoodShop.Infrastructure.Dapper.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesInfrastructureDapper(this IServiceCollection services)
        {       
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IQueryRepository, QueryRepository>();
            services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        }
    }
}
