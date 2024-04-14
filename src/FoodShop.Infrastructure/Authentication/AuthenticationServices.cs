using AutoMapper;
using Castle.Core.Configuration;
using FoodShop.Application.Services.Authentication;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using FoodShop.Domain.Abstraction.Repositories;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using FoodShop.Infrastructure.DependencyInjection.Options;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FoodShop.Contract.Abstraction.Constrant;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net.Http;
using Newtonsoft.Json;
using FoodShop.Contract.Abstraction.Shared;
namespace FoodShop.Infrastructure.Authentication
{
    
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly HttpClient _httpClient;
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly IConfigurationSection _goolgeSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public AuthenticationServices(IOptionsMonitor<JwtTokenOptions> jwtTokenOptions,
                                      IConfigurationSection goolgeSettings,
                                      UserManager<AppUser> userManager,
                                      IMapper mapper,
                                      HttpClient httpClient)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenOptions = jwtTokenOptions.CurrentValue;
            _goolgeSettings = goolgeSettings;
        }
        public async Task<AppUser> LoginWithGoogle(AuthExternalRequest model)
        {

            var returnData = new AppUser();
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { _goolgeSettings.GetSection("ClientId").Value ?? string.Empty }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);
                if (payload != null)
                {
                    var user = await _userManager.FindByEmailAsync(payload.Email);
                    returnData = user;
                }
                else
                {
                    throw new BadRequestException("Internal Server Error");
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }

            return returnData;
        
        }

        public async Task<AppUser> LoginWithFacebook(AuthExternalRequest model)
        {

            var returnData = new AppUser();
            try
            {
                HttpResponseMessage meResponse = await _httpClient.GetAsync($"https://graph.facebook.com/me?fields=first_name,last_name,email,id&access_token={model.AuthToken}");
                var userContent = await meResponse.Content.ReadAsStringAsync();
                var userContentObj = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(userContent);
                if (userContentObj != null)
                {
                    var user = await _userManager.FindByEmailAsync(userContentObj.Email);
                    if (user == null)
                        throw new BadRequestException(MessengerResult.NotFoundUser);
                    returnData = user;
                }
                else
                {
                    throw new BadRequestException("");
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }

            return returnData;

        }
        public async Task<bool> Register(string email, string password, string name, string phone)
        {
            var user = new AppUser()
            {
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                FirstName = name
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {    
                await _userManager.AddToRoleAsync(user, RoleDefine.User);
                return true; 
            }
            return false;
        }
        public async Task<bool> Login(string email, string password)
        {
            AppUser user = await _userManager.FindByEmailAsync(email)
                           ?? throw new NotFoundException(MessengerResult.EmailNotExit);
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }
        public async Task<bool> IsActiveAccountAfterRegister(string email)
        {
          
            var getData = await _userManager.FindByEmailAsync(email);
            if (getData != null)
            {
                getData.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(getData);
                if (result.Succeeded)
                    return true;
            }
            return false;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
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

    }
}
