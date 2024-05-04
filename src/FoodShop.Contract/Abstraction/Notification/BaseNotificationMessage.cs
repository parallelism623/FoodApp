using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Notification
{
    public class BaseNotificationMessage : INotificationMessage
    {
        public BaseNotificationMessage(string message) 
        {
            Message = message;
        }
        public string? Message { get; set; }
    }
}
