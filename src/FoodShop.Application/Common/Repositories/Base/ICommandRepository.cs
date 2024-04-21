using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Repositories.Base
{
    public interface ICommandRepository
    {
        Task AddAsync<T>(T entity, CancellationToken token = default);
       
        void Update<T>(T entity, CancellationToken token = default);
        void Delete<T>(T entity, CancellationToken token = default);
        void DeleteRange<T>(IEnumerable<T> entity, CancellationToken token = default);

    }
}
