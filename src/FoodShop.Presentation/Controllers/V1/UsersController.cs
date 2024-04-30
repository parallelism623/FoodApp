using FluentValidation;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Identity.Users;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.Extensions;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Infrastructure.Auth.Permission;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.Presentation.Controllers.V1
{
    public class UsersController : NatureApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserServices _userServices;
        public UsersController(
            UserManager<AppUser> userManager,
            IUserServices userServices)
        {
            _userManager = userManager;
            _userServices = userServices;
        }

        [HttpPost("confirm-email")]
        
        public async Task<IActionResult> ConfirmEmailAsync(string Token, Guid Id)
        {
            var result = await _userServices.ConfirmEmailAsync(Id, Token);
            return Ok(result);
        }
        [MustHavePermission(FSResource.Users, FSAction.Update)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request, Guid id)
        {
            var result = await _userServices.ChangePasswordAsync(request, id);
            return Ok(result);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            var result = await _userServices.ForgotPasswordAsync(email, GetOriginFromRequest());
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var result = await _userServices.LoginAsync(loginRequest);
            return Ok(result);
        }
        [MustHavePermission(FSResource.Users, FSAction.Clean)]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _userServices.LogoutAsync();
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var result = await _userServices.RegisterAsync(request, GetOriginFromRequest());
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var result = await _userServices.ResetPasswordAsync(request);
            return Ok(result);
        }
        [HttpGet]
        [MustHavePermission(FSResource.Users, FSAction.View)]
        public async Task<IActionResult> GetUserByIdAsync(Guid Id)
        {
            var result = await _userServices.GetUserByIdAsync(Id);
            return Ok(result);
        }
        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}/api/v{Request.HttpContext.GetRequestedApiVersion()}/";
    }
}
