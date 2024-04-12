using Asp.Versioning;
using FoodShop.Application.Services.Product;
using FoodShop.Application.UseCases.V1.Queries.Product;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Response.V1;
using FoodShop.Contract.Extensions;
using FoodShop.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.Presentation.Controllers.V1
{
    [ApiVersion(ApiVerions.Version1)]
    public class ProductsController : ApiController
    {

        public ProductsController(ISender sender) : base(sender) { }

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
    }
}
