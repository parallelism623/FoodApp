using AutoMapper;
using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodShop.Application.Users.Authentication
{
    public class LoginHandler : ICommandHandler<LoginCommand, UserAuthResponse>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public LoginHandler(IAuthenticationServices authenticationServices,
                            IMapper mapper,
                            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _authenticationServices = authenticationServices;
            _mapper = mapper;
        }

        public async Task<Result<UserAuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var user = await _userManager.FindByEmailAsync(model.Email) ??
                       throw new NotFoundException(MessengerResult.EmailNotExit);

            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
                throw new BadRequestException(MessengerResult.InvalidPassword);
            var isActive = await _userManager.IsEmailConfirmedAsync(user);
            if (!isActive)
            {
                var activeResult = await _userManager.ConfirmEmailAsync(user, model.TokenConfirm) ??
                                   throw new BadRequestException(MessengerResult.InvalidTokenConfirm);
            }
            var returnUser = _mapper.Map<UserAuthResponse>(user);
            returnUser.RefreshToken = _authenticationServices.GenerateRefreshToken();
            returnUser.ExpireTime = DateTime.UtcNow.AddDays(1);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, RoleDefine.User)
            };
            returnUser.AccessToken = _authenticationServices.GenerateAccessToken(claims);
            return returnUser;
        }
    }
}
