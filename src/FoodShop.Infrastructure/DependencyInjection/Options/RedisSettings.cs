using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.DependencyInjection.Options
{
    public class RedisSettings
    {
        public string Host { get; set; }
        public bool Enabled { get; set; }
    }
}
