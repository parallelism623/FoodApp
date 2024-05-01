using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Identity.Tokens;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using FoodShop.Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.Identity
{
    public class TokenServices : ITokenServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        public TokenServices(
            IConfiguration config,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _config = config;
        }
        public IEnumerable<Claim> GetClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, FSRoles.Basic)
            };
            return claims;
        }
        private string GetSecretKey()
        {
            return _config.GetRequiredSection("JwtTokenOptions").GetSection("SecretKey").Value;
        }
        public async Task<string> GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey()));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey())),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<Result<TokenRespone>> GetRefreshToken(RefreshTokenRequest request)
        {
            var userPrincipal = await GetPrincipalFromExpiredToken(request.AccessToken);
            string userEmail = userPrincipal.GetMail();
            var user = await _userManager.FindByEmailAsync(userEmail!);
            if (user is null)
            {
                throw new UnauthorizedAccessException();
            }
            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException();
            }
            user.AccessToken = await GenerateAccessToken(GetClaims(user));
            user.RefreshToken = await GenerateRefreshToken();
            await _userManager.UpdateAsync(user);
            return new TokenRespone(user.AccessToken, user.RefreshToken, user.RefreshTokenExpiryTime);
        }
    }
}
