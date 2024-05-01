using FoodShop.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Repositories.Base
{
    public interface IQueryRepository
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken token = default)
            where T : class;
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken token = default)
            where T : class;
        Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken token = default)
            where T : class;
        Task<T> IsExists<T>(Guid Id)
            where T : class;
        
    }
}
