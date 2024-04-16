    using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Abstraction.Repositories
{
    public interface IRepositoryBase<TEntity, TKey> : IDisposable
    {
        public void Add(TEntity entity);
        public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties);
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        public Task<TEntity> FindByIdAsync (TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
        public Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
        public void Remove(TEntity entity);

        public void RemoveMultiple(List<TEntity> entities);
        public void Update(TEntity entity);

    }
}
