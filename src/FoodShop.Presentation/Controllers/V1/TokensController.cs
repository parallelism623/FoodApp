
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Identity.Tokens;
using FoodShop.Presentation.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Presentation.Controllers.V1
{
    public class TokensController : NatureApiController
    {
        private readonly ITokenServices _tokenServices;
        public TokensController(ITokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }
        [HttpPost("refesh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _tokenServices.GetRefreshToken(request);
            return Ok(result);
        }
    }
}
