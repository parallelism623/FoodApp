

using FoodShop.Contract.Abstraction.Message;

namespace FoodShop.Application.Users
{
    public record DeleteUserCommand(Guid Id) : ICommand
    {
    }
}
