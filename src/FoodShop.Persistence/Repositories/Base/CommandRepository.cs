using FoodShop.Application.Common.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Persistence.Repositories.Base
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ApplicationDbContext _context;
        public CommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task AddAsync<T>(T entity, CancellationToken token = default)
        {
            await _context.AddAsync(entity);
        }

        public void Delete<T>(T entity, CancellationToken token = default)
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(IEnumerable<T> entities, CancellationToken token = default)
        {
            _context.RemoveRange(entities);
        }

        public void Update<T>(T entity, CancellationToken token = default)
        {
            _context.Update(entity);
        }

    }
}
