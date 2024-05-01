using Asp.Versioning;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Product.ProductCommand;
using FoodShop.Application.Products.ProductCommand;
using FoodShop.Application.Products.ProductQuery;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Shared;

using FoodShop.Contract.Extensions;
using FoodShop.Infrastructure.Auth.Permission;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FoodShop.Presentation.Controllers.V1
{
    [ApiVersion(ApiVerions.Version1)]

    public class ProductsController : ApiController
    {

        public ProductsController(IMediator sender) : base(sender) { }

        #region GET
        [HttpGet]
        [ProducesResponseType(typeof(Result<PagedResult<ProductResponseList>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts(string? searchTerm, string? sortColumn, string? sortOrder,
                                               string? sortOrderandColumn, int pageIndex = 1, int pageSize = 10)
        {

            var productsQuery = new GetProductsQuery(searchTerm, sortColumn,
                                                           SortOrderExtensions.ConvertStringToSortOrder(sortOrder),
                                                           SortOrderExtensions.ConvertStringToDictSortOrder(sortOrderandColumn),
                                                           pageIndex, pageSize);
            var result = await _sender.Send(productsQuery);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var productByIdQuery = new GetProductByIdQuery(id);
            var result = await _sender.Send(productByIdQuery);
            return Ok(result);
        }
        #endregion
        #region POST
        [HttpPost("{Id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProductById(Guid Id, [FromBody] CreateProductRequest request)
        {
            var productByIdCommand = new CreateProductCommand(Id, request);
            var result = await _sender.Send(productByIdCommand);
            return Ok(result);
        }
        #endregion POST

        #region PUT
        [HttpPut("{Id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductById(Guid Id, [FromBody] UpdateProductRequest request)
        {
            var productByIdCommand = new UpdateProductCommand(Id, request);
            var result = await _sender.Send(productByIdCommand);
            return Ok(result);
        }
        [HttpPut("{Id}/Category/{categoryId}")]
        [MustHavePermission(FSResource.Products, FSAction.Update)]
        public async Task<IActionResult> UpdateProductCategory(Guid id, Guid categoryId)
        {
            var productCategoryCommand = new AddProductCategoryCommand(id, categoryId);
            var result = await _sender.Send(productCategoryCommand);
            return Ok(result);
        }
        #endregion PUT
        #region DELETE
        [HttpDelete("{Id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductById(Guid Id)
        {
            var productByIdCommand = new DeleteProductCommand(Id);
            var result = await _sender.Send(productByIdCommand);
            return Ok(result);
        }
        [HttpDelete("{Id}/Category/{categoryId}")]
        [MustHavePermission(FSResource.Products, FSAction.Delete)]
        public async Task<IActionResult> DeleteProductCategory(Guid Id, Guid categoryId)
        {
            var deleteProductCategoryCommand = new DeleteProductCategoryCommand(Id, categoryId);
            var result = await _sender.Send(deleteProductCategoryCommand);
            return Ok(result);
        }
        #endregion DELETE

      

    }
}
