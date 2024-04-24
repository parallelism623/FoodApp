using Azure.Core;
using FoodShop.Application.Common.Caching;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Infrastructure.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.Auth.Permission
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ICacheServices _cacheServices;
        public PermissionAuthorizationHandler(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, ICacheServices cacheServices)
        {
            _userManager = userManager;
            _cacheServices = cacheServices;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = await _userManager.FindByEmailAsync(context.User.GetMail());
            List<FSPermission> permissions = await _cacheServices.GetCacheAsync<List<FSPermission>>($"{user.Id}:permissions");
            if (permissions == null) 
            {
                var roles = await _userManager.GetRolesAsync(user);
                var listPermission = new List<FSPermission>();
                foreach(var r in await _roleManager.Roles.AsNoTracking()
                    .Where(x => roles.Contains(x.Name)).
                    ToListAsync())
                {
                    listPermission.AddRange(r.Claims.Where(x => x.ClaimType == FSClaims.Permission)
                        .Select(x => new FSPermission(x.ClaimValue, r.Name)).ToList());
                }
                permissions = listPermission;
                _cacheServices.SetCacheAsync<List<FSPermission>>($"{user.Id}:permissions", permissions, TimeSpan.FromMinutes(2));
            }

            if (permissions.Select(x => x.Name).Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
                
            
        }
    }
}
