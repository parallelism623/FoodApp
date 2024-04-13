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
    public class UserController : ApiController
    {
        public UserController(ISender sender) : base(sender) { }

        #region GET
        [HttpGet]
        #endregion GET
        #region POST
        #endregion POST
        #region PUT
        #endregion PUT
        #region DELETE
        #endregion DELETE
    }
}
