using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.DependencyInjection.Options
{
    public class MailSettingOptions
    {
        public string ReturnPath { get; set; }
        public string Host { get; set; }
        public string SenderEmail { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiToken { get; set; }
        public string ApiBaseUrl { get; set; }
    }
}
