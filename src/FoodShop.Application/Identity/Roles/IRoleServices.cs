using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Identity.Roles
{
    public interface IRoleServices
    {
        Task<Result<RoleRespone>> GetByIdAsync(Guid id);
        Task<Result<string>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);
        Task<Result<string>> UpdatePermissionAsync(UpdatePermissionRequest request);
        Task<Result<string>> DeleteAsync(Guid Id);
    }
}
