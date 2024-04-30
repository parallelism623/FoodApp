using FoodShop.Application.Category.Command;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Infrastructure.Auth.Permission;
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
    public class CategoriesController : ApiController
    {
        public CategoriesController(IMediator sender) : base(sender)
        {
        }
        [HttpPost]
        [MustHavePermission(FSResource.Category, FSAction.Create)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var createRequest = new CreateCategoryCommand(request);
            var result = await _sender.Send(createRequest);
            return Ok(result);
        }
        [HttpDelete]
        [MustHavePermission(FSResource.Category, FSAction.Delete)]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var deleteRequest = new DeleteCategoryCommand(Id);
            var result = await _sender.Send(deleteRequest);
            return Ok(result);
        }
        [HttpPut]
        [MustHavePermission(FSResource.Category, FSAction.Update)]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            var updateRequest = new UpdateCategoryCommand(request);
            var result = await _sender.Send(updateRequest);
            return Ok(result);
        }
    }
}
