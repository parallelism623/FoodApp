using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.Presentation.Abstraction
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected readonly IMediator _sender;
        protected ApiController(IMediator sender)
        {
            _sender = sender;
        }
    }
}
