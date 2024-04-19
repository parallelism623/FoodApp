﻿using FluentValidation;
using FoodShop.Application.Behaviors;
using FoodShop.Application.Mapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
            => services.AddMediatR(cgf =>
                        {
                            cgf.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
                            cgf.AddOpenBehavior(typeof(ValidationBehavior<,>));
                        })
                        .AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);
        public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(typeof(ServiceProfile));
    }
}
