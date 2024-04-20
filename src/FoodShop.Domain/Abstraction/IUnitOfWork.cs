using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Abstraction
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransaction();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
