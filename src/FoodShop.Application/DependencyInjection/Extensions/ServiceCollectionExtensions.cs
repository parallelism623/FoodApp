using FluentValidation;
using FoodShop.Application.Common.Behaviors;
using FoodShop.Application.Common.Mapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FoodShop.Application.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
            => services.AddMediatR(cgf =>cgf.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly))
                        .AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>))
                        .AddTransient(typeof(IPipelineBehavior<,>),typeof(TransactionBehavior<,>))
                        .AddTransient(typeof(IPipelineBehavior<,>),typeof(CachingBehavior<,>))
                        .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);
        public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(typeof(ServiceProfile));

    }
}
