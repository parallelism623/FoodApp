using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvents
    {
    }
}
