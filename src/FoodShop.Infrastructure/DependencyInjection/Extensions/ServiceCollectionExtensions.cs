using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.Caching;
using FoodShop.Application.Common.Mail;
using FoodShop.Infrastructure.Auth;
using FoodShop.Infrastructure.Caching;
using FoodShop.Infrastructure.DependencyInjection.Options;
using FoodShop.Infrastructure.EmailServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.Configure<MailSettingOptions>(config.GetRequiredSection("MailSettings"))
                    .Configure<JwtTokenOptions>(config.GetRequiredSection("JwtTokenOptions"))
                    .Configure<RedisSettings>(config.GetRequiredSection("RedisSettings"))
                    .AddTransient<IAuthenticationServices, AuthenticationService>()
                    .AddTransient<IEmailServices, EmailSender>()
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
