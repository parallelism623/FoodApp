    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TokenConfirm { get; set; }
    }
}
