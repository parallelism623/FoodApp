
using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Infrastructure.DependencyInjection.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FoodShop.Infrastructure.EmailServices
{
    public class EmailSender : IEmailServices
    {
        private readonly MailSettingOptions _mailSettings;
        private readonly HttpClient _httpClient;


        public EmailSender(IOptionsMonitor<MailSettingOptions> mailSettings, IHttpClientFactory httpClientFactory)
        {
            _mailSettings = mailSettings.CurrentValue;
            _httpClient = httpClientFactory.CreateClient("MailTrapApiClient");
        }



        public async Task<bool> SendEmailAsync(HtmlMail mailData)
        {
            //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Hello.html";
            //string emailTemplateText = File.ReadAllText(filePath);

            //var htmlBody = string.Format(emailTemplateText, htmlMailData.EmailToName, DateTime.Today.Date.ToShortDateString());
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderEmail, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.Title;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.Content;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Host, int.Parse(_mailSettings.Port), MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.Username, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
