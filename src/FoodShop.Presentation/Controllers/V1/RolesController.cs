using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Identity.Roles;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Infrastructure.Auth.Permission;
using FoodShop.Presentation.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Presentation.Controllers.V1
{
    public class RolesController : NatureApiController
    {
        private readonly IRoleServices _roleServices;
        public RolesController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }   
        [HttpPost("creatorupdate-role")]
        [MustHavePermission(FSResource.UserRoles, FSAction.Update)]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateRoleRequest request)
        {
            var result = await _roleServices.CreateOrUpdateAsync(request);
            return Ok(result);
        }
        [HttpPost("update-permissions")]
        [MustHavePermission(FSResource.RoleClaims, FSAction.Update)]
        public async Task<IActionResult> UpdatePermissionAsync([FromBody] UpdatePermissionRequest request)
        {
            var result = await _roleServices.UpdatePermissionAsync(request);
            return Ok(result);
        }
        [HttpDelete]
        [MustHavePermission(FSResource.UserRoles, FSAction.Delete)]
        public async Task<IActionResult> CreateOrUpdate(Guid id)
        {
            var result = await _roleServices.DeleteAsync(id);
            return Ok(result);
        }
    }
}
