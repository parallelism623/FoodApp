using Castle.Core.Configuration;
using Castle.Core.Smtp;
using FoodShop.Application.Services.Authentication;
using FoodShop.Application.Services.Mail;
using FoodShop.Infrastructure.Authentication;
using FoodShop.Infrastructure.DependencyInjection.Options;
using FoodShop.Infrastructure.EmailServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtTokenConfiguration(this IServiceCollection services, IConfigurationSection section)
        {
            services.AddOptions<JwtTokenOptions>(section.Value);
            services.AddAuthentication(opt =>
                {
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["SecretKey"]))
                    };
                });
        }
        public static void AddMailServiceConfiguration(this IServiceCollection services, IConfigurationSection mailSettings)
        {
            services.AddHttpClient("MailTrapApiClient", (services, client) =>
            {
                MailSettingOptions _mailSettings = new MailSettingOptions();
                mailSettings.Bind(_mailSettings);
                client.BaseAddress = new Uri(_mailSettings.ApiBaseUrl);
                client.DefaultRequestHeaders.Add("Api-Token", _mailSettings.ApiToken);
            });
        }
        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationServices, AuthenticationServices>()
                    .AddScoped<IEmailServices, EmailSender>();
        }
    }
}
