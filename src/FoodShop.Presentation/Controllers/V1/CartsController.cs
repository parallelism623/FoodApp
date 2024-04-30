using FoodShop.Application.Cart.CartCommand;
using FoodShop.Application.Cart.CartQuery;
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
    public class CartController : ApiController
    {
        public CartController(IMediator sender) : base(sender)
        {
        }
        [HttpGet]
        [MustHavePermission(FSResource.Cart, FSAction.View)]
        public async Task<IActionResult> GetCart()
        {
            var getCartQuery = new GetCartQuery();
            var result = await _sender.Send(getCartQuery);
            return Ok(result);
        }
        [HttpPost("add-product")]
        [MustHavePermission(FSResource.Cart, FSAction.Update)]
        public async Task<IActionResult> AddProduct(Guid ProductId)
        {
            var addProductQuery = new AddProductCommand(ProductId);
            var result = await _sender.Send(addProductQuery);
            return Ok(result);
        }

        [HttpPost("delete-products")]
        [MustHavePermission(FSResource.Cart, FSAction.Delete)]
        public async Task<IActionResult> AddProduct([FromBody] List<Guid> ProductId)
        {
            var deleteProductsQuery = new DeleteProductsCommand(ProductId);
            var result = await _sender.Send(deleteProductsQuery);
            return Ok(result);
        }
    }

}
