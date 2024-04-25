using FoodShop.Contract.Abstraction.Authorization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.Auth.Permission
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string resource, string action)
        {
            Policy = FSPermission.NameFor(action, resource);
        }
    }
}
