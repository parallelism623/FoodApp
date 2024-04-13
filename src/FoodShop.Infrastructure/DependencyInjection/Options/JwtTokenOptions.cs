using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.DependencyInjection.Options
{
    public class JwtTokenOptions
    {
        public string SecretKey { get; set; }   
        public string Issuer { get;set; }
        public int ExpireMinute { get; set; }
        public int ExpiresSecond { get; set; }
        public int ExpiresHour { get; set; }

    }
}
