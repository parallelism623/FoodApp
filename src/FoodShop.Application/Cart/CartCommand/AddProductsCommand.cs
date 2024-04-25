using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FoodShop.Application.Cart.CartCommand
{
    public record AddProductsCommand(Guid ProductId) : ICommand
    {
    }
}
