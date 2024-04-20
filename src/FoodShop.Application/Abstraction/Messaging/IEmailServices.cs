using FoodShop.Contract.Abstraction.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Abstraction.Messaging
{
    public interface IEmailServices
    {
        Task<bool> SendEmailAsync(HtmlMail htmlMailData);
    }
}
