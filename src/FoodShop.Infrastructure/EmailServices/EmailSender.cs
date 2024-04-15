using Castle.Core.Configuration;
using Castle.Core.Smtp;
using FoodShop.Application.Services.Mail;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Infrastructure.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.EmailServices
{
    public class EmailSender : IEmailServices
    {
        private readonly MailSettingOptions _mailSettings;
        private readonly HttpClient _httpClient;

        public EmailSender(IConfigurationSection mailSettings, IHttpClientFactory httpClientFactory)
        {
            mailSettings.Bind(_mailSettings);
            _httpClient = httpClientFactory.CreateClient("MailTrapApiClient");
        }



        public async Task<bool> SendEmailAsync(HtmlMail htmlMailData)
        {
            //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Hello.html";
            //string emailTemplateText = File.ReadAllText(filePath);

            //var htmlBody = string.Format(emailTemplateText, htmlMailData.EmailToName, DateTime.Today.Date.ToShortDateString());

            var apiEmail = new
            {
                From = new { Email = _mailSettings.SenderEmail, Name = _mailSettings.SenderEmail },
                To = new[] { new { Email = htmlMailData.EmailToId, Name = htmlMailData.EmailToName } },
                Subject = "Hello",
                Text = htmlMailData.Content
            };

            var httpResponse = await _httpClient.PostAsJsonAsync("send", apiEmail);

            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);

            if (response != null && response.TryGetValue("success", out object? success) && success is bool boolSuccess && boolSuccess)
            {
                return true;
            }

            return false;
        }
    }
}
