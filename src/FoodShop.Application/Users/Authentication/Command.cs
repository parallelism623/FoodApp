using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Users.Authentication
{
    public record LoginWithGoogleCommand(AuthExternalRequest Model) : ICommand<UserAuthResponse>;
    public record RegisterCommand(RegisterRequest Model) : ICommand<UserAuthResponse>;
    public record LoginCommand(LoginRequest Model) : ICommand<UserAuthResponse>;
    public record LoginWithFacebookCommand(AuthExternalRequest Model) : ICommand<UserAuthResponse>;
}
