using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodShop.Application.Category.Command
{
    public record DeleteCategoryCommand(Guid CategoryId) : ICommand<string>;
}
