using Asp.Versioning;
using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Application.Services.Authentication;
using FoodShop.Application.Services.Mail;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using FoodShop.Infrastructure.Authentication;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Presentation.Controllers.V1
{
    [ApiVersion(ApiVerions.Version1)]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationServices _authenticationServices;
        public AuthenticationController(IMediator sender, IAuthenticationServices authenticationServices) : base(sender) 
        {
            _authenticationServices = authenticationServices;   
        }
      
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] AuthExternalRequest model)
        {
            var loginGGCommand = new LoginWithGoogleCommand(model);
            var result = await _sender.Send(loginGGCommand);
            return Ok(result);
        }
        [HttpPost("login-facebook")]
        public async Task<IActionResult> LoginWithFaceBook([FromBody] AuthExternalRequest model)
        {
            var loginFacebookCommand = new LoginWithFacebookCommand(model);
            var result = await _sender.Send(loginFacebookCommand);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var loginCommand = new LoginCommand(model);
            var result = await _sender.Send(loginCommand);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var registerCommand = new RegisterCommand(model);
            var result = await _sender.Send(registerCommand);
            if (result.IsSuccess)
            {
                _sender.Publish(new EmailSenderEvent(model));
            }
            return Ok(result);
        }
        ////[HttpPost("logout")]
        ////public Task<Result<UserAuthResponse>> Login([FromBody] AuthExternalRequest model)
        ////{

        ////}

    }
}
