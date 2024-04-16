using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Request.V1;


namespace FoodShop.Application.Services.Mail
{
    public record EmailSenderEvent(RegisterRequest Model) : IDomainEvents;
}
