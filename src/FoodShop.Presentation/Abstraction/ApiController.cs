using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.Presentation.Abstraction
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected readonly ISender _sender;
        protected ApiController(ISender sender)
        {
            _sender = sender;
        }
    }
}
