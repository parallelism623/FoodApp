using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.Caching;
using FoodShop.Application.Common.Exceptions;
using FoodShop.Application.Common.Mail;
using FoodShop.Application.Identity.Tokens;
using FoodShop.Application.Identity.Users;
using FoodShop.Domain.Exceptions;
using FoodShop.Infrastructure.Auth;
using FoodShop.Infrastructure.Auth.Permission;
using FoodShop.Infrastructure.Caching;
using FoodShop.Infrastructure.DependencyInjection.Options;
using FoodShop.Infrastructure.EmailServices;
using FoodShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
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
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,opt =>
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
            services.Configure<MailSettingOptions>(config.GetRequiredSection("MailSettings"))
                    .Configure<JwtTokenOptions>(config.GetRequiredSection("JwtTokenOptions"))
                    .Configure<RedisSettings>(config.GetRequiredSection("RedisSettings"))
                    .AddTransient<ITokenServices, TokenServices>()
                    .AddTransient<IUserServices, UserServices>()
                    .AddTransient<IEmailServices, EmailSender>()
                    .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                    .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                    
                    .AddHttpClient()
                    .AddScoped<ICurrentUser, CurrentUser>()
                    .AddScoped<ICurrentUserInitialize, CurrentUser>()
                    .AddScoped<CurrentUserMiddleware>();
        }
        public static void AddDistributedCacheConfig(this IServiceCollection services, IConfigurationSection redisSettings)
        {
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisSettings["Host"]));
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = redisSettings["Host"];
                opt.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    AbortOnConnectFail = true
                };
            });
            services.AddScoped<ICacheServices, CacheServices>();
        }
    }
}
