using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Orders;
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
    public class OrdersController : ApiController
    {
        public OrdersController(IMediator sender) : base(sender) { }
        #region GET
        [HttpGet]
        [MustHavePermission(FSResource.Order, FSAction.Delete)]
        public async Task<IActionResult> GetOrders([FromBody] PagingListRequest request)
        {
            var getOrdersQuery = new GetOrdersQuery(request);
            var result = await _sender.Send(getOrdersQuery);
            return Ok(result);

        }
        [HttpGet("user/{Id}")]
        [MustHavePermission(FSResource.Order, FSAction.View)]
        public async Task<IActionResult> GetOrdersByUserId(Guid id, [FromBody] PagingListRequest request)
        {
            var getOrdersByUserIdQuery = new GetOrdersByUserIdQuery(id, request);
            var result = await _sender.Send(request);
            return Ok(result);
        }
        [HttpGet("{Id}")]
        [MustHavePermission(FSResource.Order, FSAction.View)]
        public async Task<IActionResult> GetOrderById(Guid Id)
        {
            var getOrderById = new GetOrderByIdRequest(Id);
            var result = await _sender.Send(getOrderById);
            return Ok(result);
        }
        #endregion GET
        #region POST
        [HttpPost]
        [MustHavePermission(FSResource.Order, FSAction.Create)]
        public async Task<IActionResult> CreateOrders([FromBody] CreateOrderRequest request)
        {
            var createOrderRequest = new CreateOrderCommand(request);
            var result = await _sender.Send(createOrderRequest);
            return Ok(result);
        }
        #endregion POST
        #region PUT
        #endregion PUT
        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var deleteOrderCommand = new DeleteOrderCommand(id);
            var result = await _sender.Send(deleteOrderCommand);
            return Ok(result);
        }
        #endregion DELETE
    }
}
