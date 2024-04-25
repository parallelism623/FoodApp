using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Presentation.Abstraction
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NatureApiController : ControllerBase
    {
    }
}
