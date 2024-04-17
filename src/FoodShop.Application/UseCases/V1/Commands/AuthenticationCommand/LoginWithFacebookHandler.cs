using AutoMapper;
using FoodShop.Application.Services.Authentication;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodShop.Application.UseCases.V1.Commands.AuthenticationCommand
{
    public class LoginWithFacebookHandler : ICommandHandler<LoginWithFacebookCommand, UserAuthResponse>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;
        public LoginWithFacebookHandler(IAuthenticationServices authenticationServices, IMapper mapper)
        {
            _authenticationServices = authenticationServices;
            _mapper = mapper;
        }

        public async Task<Result<UserAuthResponse>> Handle(LoginWithFacebookCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var result = await _authenticationServices.LoginWithFacebook(model)
                         ?? throw new BadRequestException("Invalid token"); // update
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
