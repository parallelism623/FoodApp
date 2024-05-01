using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodShop.Application.Product.ProductCommand
{
    public record DeleteProductCategoryCommand(Guid ProductId, Guid CategoryId) : ICommand<string>
    {
    }
}
