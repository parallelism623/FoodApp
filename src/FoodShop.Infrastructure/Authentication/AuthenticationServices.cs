using AutoMapper;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using FoodShop.Infrastructure.DependencyInjection.Options;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoodShop.Contract.Abstraction.Constrant;
using Newtonsoft.Json;
using System.Web;
using System;
using FoodShop.Application.Abstraction.Messaging;
namespace FoodShop.Infrastructure.Authentication
{

    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly HttpClient _httpClient;
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly IConfigurationSection _goolgeSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly MailSettingOptions _mailSettings;
        public AuthenticationServices(IOptionsMonitor<JwtTokenOptions> jwtTokenOptions,
                                      IConfiguration goolgeSettings,
                                      UserManager<AppUser> userManager,
                                      IMapper mapper,
                                      HttpClient httpClient,
                                      IOptionsMonitor<MailSettingOptions> mailSettings)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenOptions = jwtTokenOptions.CurrentValue;
            _goolgeSettings = goolgeSettings.GetRequiredSection("GoogleAuthSettings");
            _mailSettings = mailSettings.CurrentValue;
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
        public async Task<AppUser> Register(RegisterRequest model)
        {
            var user = _mapper.Map<AppUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {    
                await _userManager.AddToRoleAsync(user, RoleDefine.User);
                return user; 
            }
            return null;
        }
        public async Task<bool> Login(string email, string password)
        {
            AppUser user = await _userManager.FindByEmailAsync(email)
                           ?? throw new NotFoundException(MessengerResult.EmailNotExit);
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }
        public async Task<bool> IsActiveAccountAfterRegister(string tokenConfirm, string Email)
        {

            var getData = await _userManager.FindByEmailAsync(Email)
                          ?? throw new NotFoundException(MessengerResult.EmailNotExit);
            if (getData != null)
            {
                var result = await _userManager.ConfirmEmailAsync(getData, tokenConfirm);
                if (result.Succeeded)
                    return true;
                else
                    throw new BadRequestException("Token confirm email is not valid");
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey)),
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
        public async Task<string> GenerateTokenComfirmMail(string email)
            {
            var user = await _userManager.FindByEmailAsync(email)
                       ?? throw new NotFoundException(MessengerResult.EmailNotExit);
            var tokenConfirmEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            UriBuilder uriBuilder = new UriBuilder(_mailSettings.ReturnPath);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = tokenConfirmEmail;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();
            return urlString;
        }


    }
}
