using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.DataTransferObjects.Request.V1
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
 
    }
}
