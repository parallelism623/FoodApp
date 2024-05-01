using Dapper;
using FoodShop.Application.Common.Dapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Domain.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace FoodShop.Persistence.Repositories.Base
{
    public class QueryRepository : IQueryRepository
    {
        private readonly IDbConnectionFactory _dapperContext;
        public QueryRepository(IDbConnectionFactory dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<T> IsExists<T>(Guid Id)
            where T : class
        {
            var stringSql = $"SELECT * FROM {nameof(T)} WHERE Id = {Id}";
            return await QueryFirstOrDefaultAsync<T>(stringSql);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken token = default)
        where T : class
        {
            using var connection = _dapperContext.CreateConnection() ;
            return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
            
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken token = default)
        where T : class
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken token = default)
        where T : class
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleAsync<T>(sql, param, transaction);
        }
    }
}
