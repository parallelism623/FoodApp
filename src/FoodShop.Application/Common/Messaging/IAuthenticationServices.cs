using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Abstraction.Messaging
{
    public interface IAuthenticationServices
    {
        public Task<AppUser> LoginWithFacebook(AuthExternalRequest model);
        public Task<AppUser> LoginWithGoogle(AuthExternalRequest model);
        public Task<string> GenerateTokenComfirmMail(string email);
        public string GenerateAccessToken(IEnumerable<Claim> claims);
        public string GenerateRefreshToken();
        public Task<bool> IsActiveAccountAfterRegister(string tokenConfirm, string email);
        public Task<AppUser> Register(RegisterRequest model);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);


    }
}
