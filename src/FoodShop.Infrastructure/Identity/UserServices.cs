using AutoMapper;
using FluentValidation;
using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.Caching;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Exceptions;
using FoodShop.Application.Common.Mail;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Application.Identity.Tokens;
using FoodShop.Application.Identity.Users;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodShop.Infrastructure.Identity
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenServices _tokenServices;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IQueryRepository _queryRepository;
        private readonly ICurrentUser _userCurrent;
        private readonly IEmailServices _emailServices;
        private readonly ICacheServices _cacheServices;

        public UserServices(
         
            IQueryRepository queryRepository,
            UserManager<AppUser> userManager, 
            ITokenServices tokenServices,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ICurrentUser user,
            IEmailServices emailServices)
        {
            _userCurrent = user;
            _userManager = userManager;
            _tokenServices = tokenServices;
            _signInManager = signInManager;
            _mapper = mapper;
            _queryRepository = queryRepository;
            _emailServices = emailServices;
        }
        public Task<Result<string>> AssignRolesAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString())
                       ?? throw new NotFoundException($"User can not find by id is {id}");
            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            if (!result.Succeeded)
                throw new InternalServerException("Change password failed");
            return "Required for changing password has been excuted successful";
        }

        public async Task<Result<string>> ConfirmEmailAsync(Guid id, string code, CancellationToken token = default)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new BadRequestException(MessengerResult.NotFoundUser);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                throw new InternalServerException(MessengerResult.InvalidTokenConfirm);
            return "Account confirmed for Email. Please login";
        }

        public async Task<Result<string>> ForgotPasswordAsync(string email, string origin)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new BadRequestException("An Error has occurred!");
            }
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            const string route = "user/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new SendMailRequest
            {
                EmailToId = email,
                EmailToName = email,
                Title = "Reset Password",
                Content = $"Your Password Reset Token is '{code}'. You can reset your password using the {endpointUri} Endpoint."
            };
            _emailServices.SendEmailAsync(mailRequest);
            return "Password Reset Mail has been sent to your authorized Email";
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(Guid id, CancellationToken token = default)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(token) ??
                throw new NotFoundException("User not found");
            var returnUser = _mapper.Map<UserResponse>(user);
            return returnUser;
        }

        public Task<Result<PagedResult<UserResponseList>>> GetUsersAsync(ListRequest request, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserAuthResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken token = default)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new BadRequestException("An Error has occured");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            if (!result.Succeeded)
            {
                throw new BadRequestException(MessengerResult.InvalidPassword);
            }
            var claims = _tokenServices.GetClaims(user);
            user.AccessToken = await _tokenServices.GenerateAccessToken(claims);
            user.RefreshToken = await _tokenServices.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            var resultUpdate =  await _userManager.UpdateAsync(user);
          
            var returnUser = _mapper.Map<UserAuthResponse>(user); 
            return returnUser;
        }

        public async Task<Result> LogoutAsync()
        {
            var user = await _userManager.FindByEmailAsync(_userCurrent.GetUserId().ToString());
            if (user is null)
            {
                throw new BadRequestException("An Error has occurred!");
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            await _cacheServices.SetCacheAsync($"{user.Email}:{user.AccessToken}", user.AccessToken, TimeSpan.FromMinutes(1));
            return Result.Success();
        }

        public async Task<Result<UserAuthResponse>> RegisterAsync(RegisterRequest request, string origin, CancellationToken token = default)
        {
            var validator = new RegisterRequestValidation();
            await validator.ValidateAndThrowAsync(request);
            var result = await _userManager.FindByEmailAsync(request.Email);
            if (result is not null)
            {
                throw new BadRequestException(MessengerResult.EmailExit);
            }
            var user = _mapper.Map<AppUser>(request);
            var resultCreate = await _userManager.CreateAsync(user, request.Password);
            if (!resultCreate.Succeeded)
            {
                throw new InternalServerException("An Error has occured");
            }
            await _userManager.AddToRoleAsync(user, FSRoles.Basic);
            user.AccessToken = await _tokenServices.GenerateAccessToken(_tokenServices.GetClaims(user));
            user.RefreshToken = await _tokenServices.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var uri = new Uri(string.Concat($"{origin}","user/confirm-email"));
            var url = QueryHelpers.AddQueryString(uri.ToString(), new Dictionary<string, string> { { "Token", code}, { "Id", user.Id.ToString()} });
            var emailRequest = new SendMailRequest
            {
                EmailToId = request.Email,
                EmailToName = request.Email,
                Title = "Confirm Email",
                Content = $"You has registed an account by this Email. Click on {url} to confirm email"
            };
            _emailServices.SendEmailAsync(emailRequest);
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserAuthResponse>(user); 
        }

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken token = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new BadRequestException(MessengerResult.NotFoundUser);
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
                throw new InternalServerException("An Error has occurred!");
            return "Password reset successful";
        }

        public Task UpdateAsync(UpdateUserRequest request, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
