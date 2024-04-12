using FoodShop.Domain.Abstraction.Repositories;
using FoodShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace FoodShop.Persistence.Repositories
{
    public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
    }
}
