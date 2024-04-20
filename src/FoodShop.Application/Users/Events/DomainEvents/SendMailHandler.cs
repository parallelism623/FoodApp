using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Users.Events.DomainEvents
{
    public class SendMailHandler : IDomainEventHandler<EmailSenderEvent>
    {
        private readonly IEmailServices _emailServices;
        private readonly IAuthenticationServices _authenticationServices;
        public SendMailHandler(IEmailServices emailServices, IAuthenticationServices authenticationServices)
        {
            _emailServices = emailServices;
            _authenticationServices = authenticationServices;
        }

        public async Task Handle(EmailSenderEvent notification, CancellationToken cancellationToken)
        {
            var model = notification.Model;
            var data = new HtmlMail();
            data.EmailToName = model.Email;
            data.EmailToId = model.Email;
            data.Title = "NUll";
            data.Content = await _authenticationServices.GenerateTokenComfirmMail(model.Email);
            await _emailServices.SendEmailAsync(data);
        }
    }
}
