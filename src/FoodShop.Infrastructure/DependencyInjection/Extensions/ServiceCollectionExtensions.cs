using FoodShop.Application.Services.Authentication;
using FoodShop.Application.Services.Mail;
using FoodShop.Infrastructure.Authentication;
using FoodShop.Infrastructure.DependencyInjection.Options;
using FoodShop.Infrastructure.EmailServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        public static void AddExternalServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            services.Configure<MailSettingOptions>(config.GetRequiredSection("MailSettings"));
            services.AddTransient<IAuthenticationServices, AuthenticationServices>()
                    .AddTransient<IEmailServices, EmailSender>()
                    .AddHttpClient();
        }
    }
}
