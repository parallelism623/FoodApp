using Asp.Versioning;
using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Users.Authentication;
using FoodShop.Application.Users.Events.DomainEvents;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
