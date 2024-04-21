using AutoMapper;
using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Exceptions;
using System.Security.Claims;

namespace FoodShop.Application.Users.Authentication
{
    public class LoginWithGoogleHandler : ICommandHandler<LoginWithGoogleCommand, UserAuthResponse>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;
        public LoginWithGoogleHandler(IAuthenticationServices authenticationServices,
                                      IMapper mapper)
        {
            _mapper = mapper;
            _authenticationServices = authenticationServices;
        }
        public async Task<Result<UserAuthResponse>> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationServices.LoginWithGoogle(request.Model)
                         ?? throw new NotFoundException(MessengerResult.EmailNotExit);
            var user = _mapper.Map<UserAuthResponse>(result);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, RoleDefine.User)
            };
            user.AccessToken = _authenticationServices.GenerateAccessToken(claims);
            user.RefreshToken = _authenticationServices.GenerateRefreshToken();
            user.ExpireTime = DateTime.UtcNow.AddDays(1);
            return user;
        }
    }
}
