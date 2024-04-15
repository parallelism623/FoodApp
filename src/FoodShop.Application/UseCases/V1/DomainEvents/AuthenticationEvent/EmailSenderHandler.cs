using Castle.Core.Smtp;
using FoodShop.Application.Services.Authentication;
using FoodShop.Application.Services.Mail;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.UseCases.V1.DomainEvents.AuthenticationEvent
{
    public class EmailSenderHandler : IDomainEventHandler<EmailSenderEvent>
    {
        private readonly IEmailServices _emailServices;
        private readonly IAuthenticationServices _authenticationServices;
        public EmailSenderHandler(IEmailServices emailServices, IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
            _emailServices = emailServices;
        }
        public async Task Handle(EmailSenderEvent notification, CancellationToken cancellationToken)
        {
            var model = notification.Model;
            var htmlData = new HtmlMail();
            htmlData.Title = "Confirm Email";
            htmlData.Content = await _authenticationServices.GenerateTokenComfirmMail(model.Email);
            var result = await _emailServices.SendEmailAsync(htmlData);
            
        }
    }
}
