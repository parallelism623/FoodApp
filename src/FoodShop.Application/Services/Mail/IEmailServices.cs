using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Services.Mail
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string toEmail, string subjectEmail, string bodyEmail);
    }
}
