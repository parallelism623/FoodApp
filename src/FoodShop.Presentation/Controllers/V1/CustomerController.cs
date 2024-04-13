using FoodShop.Presentation.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Presentation.Controllers.V1
{
    public class CustomerController : ApiController
    {
        public CustomerController(ISender sender) : base(sender) { }
    }
}
