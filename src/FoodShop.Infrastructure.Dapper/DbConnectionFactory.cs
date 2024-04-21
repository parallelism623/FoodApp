
using FoodShop.Application.Common.Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using Microsoft.Extensions.Configuration;
namespace FoodShop.Infrastructure.Dapper
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection CreateConnection()
        {
            string _connectionString = _config.GetConnectionString("DefaultConnection")!;
            return new SqliteConnection(_connectionString);
        }
    }
}
