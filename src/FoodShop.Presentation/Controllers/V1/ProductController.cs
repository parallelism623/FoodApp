using Asp.Versioning;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Presentation.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Presentation.Controllers.V1
{
    [ApiVersion(ApiVerions.Version1)]
    public class ProductController : ApiController
    {

        public ProductController(ISender sender) : base(sender) { }

    }
}
