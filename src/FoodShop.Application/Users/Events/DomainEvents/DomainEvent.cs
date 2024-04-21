using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;


namespace FoodShop.Application.Users.Events.DomainEvents
{
    public record EmailSenderEvent(RegisterRequest Model) : IDomainEvents;
}
