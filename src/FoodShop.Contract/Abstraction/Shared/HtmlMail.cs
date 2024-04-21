using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Shared
{
    public class HtmlMail
    {
    
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string Content { get; set; }
        public string Title { get; set;}

    }
}
