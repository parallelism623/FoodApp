using Asp.Versioning;
using FoodShop.Application.Services.Authentication;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
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
        public AuthenticationController(ISender sender) : base(sender) { }
      
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] AuthExternalRequest model)
        {
            var loginGGCommand = new LoginWithGoogleCommand(model);
            var result = await _sender.Send(loginGGCommand);
            return Ok(result);
        }
        [HttpPost("login-facebook")]
        public Task<Result<UserAuthResponse>> LoginWithFaceBook([FromBody] AuthExternalRequest model)
        {

        }
        [HttpPost("login")]
        public Task<Result<UserAuthResponse>> Login([FromBody] LoginRequest model)
        {

        }
        [HttpPost("register")]
        public Task<Result<UserAuthResponse>> Register([FromBody] LoginRequest model)
        {
            var registerCommand = new RegisterCommand(model);
            var result = await _sender.Send(registerCommand);
            return Ok(result);
        }
        [HttpPost("logout")]
        public Task<Result<UserAuthResponse>> Login([FromBody] AuthExternalRequest model)
        {

        }
    }
}
