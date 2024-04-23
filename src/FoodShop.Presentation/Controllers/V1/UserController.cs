using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Users;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.Extensions;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.Presentation.Controllers.V1
{
    public class UserController : ApiController
    {
        public UserController(IMediator sender) : base(sender) { }

        #region GET
        [HttpGet]
        [ProducesResponseType(typeof(Result<PagedResult<UserResponseList>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers(string? searchTerm, string? sortColumn, string? sortOrder,
                                               string? sortOrderandColumn, int pageIndex = 1, int pageSize = 10)
        {

            var usersQuery = new GetUsersQuery(searchTerm, sortColumn,
                                                           SortOrderExtensions.ConvertStringToSortOrder(sortOrder),
                                                           SortOrderExtensions.ConvertStringToDictSortOrder(sortOrderandColumn),
                                                           pageIndex, pageSize);
            var result = await _sender.Send(usersQuery);
            return Ok(result);
        }
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(Result<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var userQuery = new GetUserByIdQuery(Id);
            var result = await _sender.Send(userQuery);
            return Ok(result);
        }
        #endregion GET
        #region POST
        #endregion POST
        #region PUT
        #endregion PUT
        #region DELETE
        [HttpDelete("{Id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var userQuery = new DeleteUserCommand(Id);
            var result = await _sender.Send(userQuery);
            return Ok(result);
        }
        #endregion DELETE
    }
}
