using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodShop.Application.Cart.CartCommand
{
    public record DeleteProductsCommand (List<Guid> ProductsId) : ICommand<string>;
}
