using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Services.Authentication
{
    public record LoginWithGoogleCommand (AuthExternalRequest Model): ICommand<UserAuthResponse>;
    public record RegisterCommand (RegisterRequest Model) : ICommand<UserAuthResponse>;
}
