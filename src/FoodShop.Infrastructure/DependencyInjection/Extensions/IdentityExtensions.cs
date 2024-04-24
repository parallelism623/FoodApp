using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.DependencyInjection.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetMail(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(x => x.Equals(ClaimTypes.Email)).Value;
        }
        public static string GetId(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(x => x.Equals("Id")).Value;
        }
    }
}
