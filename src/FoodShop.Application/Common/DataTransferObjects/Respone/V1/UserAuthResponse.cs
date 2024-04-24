using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Respone.V1
{
    public class UserAuthResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }

        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }


        public string? Avartar { get; set; }

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
