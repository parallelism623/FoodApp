using Asp.Versioning;
using FoodShop.Application.Products.ProductQuery;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Response.V1;

using FoodShop.Contract.Extensions;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FoodShop.Presentation.Controllers.V1
{
    [ApiVersion(ApiVerions.Version1)]
    
    public class ProductController : ApiController
    {

        public ProductController(IMediator sender) : base(sender) { }

        #region GET
        [HttpGet]
        [ProducesResponseType(typeof(Result<PagedResult<ProductResponseList>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts(string? searchTerm, string? sortColumn, string? sortOrder,
                                               string? sortOrderandColumn, int pageIndex = 1, int pageSize = 10) 
        {
       
            var productsQuery = new Query.GetProductsQuery(searchTerm, sortColumn,
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
            var productByIdQuery = new Query.GetProductByIdQuery(id);
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
        #endregion PUT
        #region DELETE
        [HttpDelete("{Id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductById(Guid Id)
        {
            var productByIdCommand = new DeleteProductCommand(Id);
            var result = await _sender.Send(productByIdCommand);
            return Ok(result);
        }
        #endregion DELETE

    }
}
