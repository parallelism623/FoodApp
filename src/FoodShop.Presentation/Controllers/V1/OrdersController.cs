using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Orders;
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
        public OrdersController(IMediator sender) : base(sender)
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(PagingListRequest request)
        {
            var getOrdersQuery = new GetOrdersQuery(request);
            var result = await _sender.Send(getOrdersQuery);
            return Ok(result);

        }

        //[HttpGet("user/{Id}")]
        //public async Task<IActionResult> GetOrdersByUserId(Guid id)
        //{

        //}
    }
}
