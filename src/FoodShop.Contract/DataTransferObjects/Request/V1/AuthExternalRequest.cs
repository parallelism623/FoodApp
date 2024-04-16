using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.DataTransferObjects.Request.V1
{   public class AuthExternalRequest
    {
        public string? Provider { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Username => Email;
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AuthToken { get; set; }
        public string? IdToken { get; set; }
        public string? AuthorizationCode { get; set; }
        public dynamic? Response { get; set; }
    }
}
