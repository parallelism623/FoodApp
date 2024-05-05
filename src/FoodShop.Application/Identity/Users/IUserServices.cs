using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Identity.Users
{
    public interface IUserServices
    {
        Task IsUserExists(Guid Id);
        Task<Result<UserAuthResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken token = default);
        Task<Result> LogoutAsync();
        Task<Result<PagedResult<UserResponseList>>> GetUsersAsync(ListRequest request, CancellationToken token = default);
        Task<Result<UserResponse>> GetUserByIdAsync(Guid id, CancellationToken token = default);
        Task<Result<string>> AssignRolesAsync(Guid id, CancellationToken token = default);
        Task<Result<UserAuthResponse>> RegisterAsync(RegisterRequest request, string origin, CancellationToken token = default);
        Task UpdateAsync(UpdateUserRequest request, CancellationToken token = default);
        Task<Result<string>> ConfirmEmailAsync(Guid id, string code, CancellationToken token = default);
        Task<Result<string>> ForgotPasswordAsync(string email, string origin);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken token = default );
        Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, Guid id);
    }
}
