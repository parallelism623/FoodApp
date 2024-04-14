using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Services.Authentication
{
    public interface IAuthenticationServices
    {
        public Task<AppUser> LoginWithFacebook(AuthExternalRequest model);
        public Task<AppUser> LoginWithGoogle(AuthExternalRequest model);
        public Task<bool> IsActiveAccountAfterRegister(string email);
        public string GenerateAccessToken(IEnumerable<Claim> claims);
        public string GenerateRefreshToken();
        public Task<bool> Register(string email, string password, string name, string phone)
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
