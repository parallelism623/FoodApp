using Asp.Versioning;
using FoodShop.API;
using FoodShop.API.Middleware;
using FoodShop.Application.DependencyInjection.Extensions;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Infrastructure.Auth;
using FoodShop.Infrastructure.Dapper.DependencyInjection.Extensions;
using FoodShop.Infrastructure.DependencyInjection;
using FoodShop.Persistence;
using FoodShop.Persistence.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();
// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
#region AddServices
builder
    .Services.AddServicesInfrastructureDapper();
builder
    .Services.AddDistributedCacheConfig(builder.Configuration.GetRequiredSection("RedisSettings"));
builder
    .Services.AddSqlConfiguration();

builder
    .Services.AddConfigureMediatR()
                .AddConfigureAutoMapper()
                .AddRepositoriesConfiguration();
builder
    .Services
    .AddControllers()
    .AddApplicationPart(FoodShop.Presentation.AssemblyReference.Assembly);
builder
    .Services.AddJwtTokenConfiguration(builder.Configuration.GetRequiredSection("JwtTokenOptions"));
builder
    .Services.AddExternalServices(builder.Configuration);
builder
    .Services.AddConfigureAutoMapper();
#endregion AddServices

#region VersionApiController
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
}).AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});
#endregion VersionApiController
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<CurrentUserMiddleware>();
app.UseAuthorization();
app.MigrateDatabase();
app.AddMapHubRoute();
app.MapControllers();

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
