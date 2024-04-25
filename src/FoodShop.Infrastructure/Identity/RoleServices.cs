using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Exceptions;
using FoodShop.Application.Identity.Roles;
using FoodShop.Contract.Abstraction.Authorization;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using FoodShop.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace FoodShop.Infrastructure.Identity
{
    public class RoleServices : IRoleServices
    {
        private readonly ICurrentUser _currentUser;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        public RoleServices(ICurrentUser currentUser, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _currentUser = currentUser;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result<string>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request)
        {
            if (request.Id is null)
            {
                var newRole = new AppRole
                {
                    Name = request.Name, 
                    Description = request.Description,
                    NormalizedName =  request.Name.ToUpper(),
                };
                var result = await _roleManager.CreateAsync(newRole);
                if (!result.Succeeded)
                {
                    throw new InternalServerException("An Error has occured");
                }
                return "Add new role successful";
            }
            else
            {
                var role = await _roleManager.FindByIdAsync(request.Id.ToString());
                if (role == null)
                {
                    throw new NotFoundException("Role Not Found");
                }
                if (request.Name is null)
                {
                    throw new BadRequestException("Role's Name doesn't able to empty");
                }
                role.Name = request.Name;
                role.NormalizedName = request.Name.ToUpperInvariant();
                role.Description = request.Description;
                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    throw new InternalServerException("An Error has occured");
                }
                return "Update new role successful";
            }
        }

        public async Task<Result<string>>  DeleteAsync(Guid Id)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                throw new NotFoundException("Role Not Found");
            }
            if ((await _userManager.GetUsersInRoleAsync(role.Name)).Count > 0)
            {
                throw new BadRequestException("Not allowed delete this role as it is being used");
            }
            await _roleManager.DeleteAsync(role);
            return "Delete Role Successful";
        }

        public Task<Result<RoleRespone>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> UpdatePermissionAsync(UpdatePermissionRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString())
                ?? throw new NotFoundException("Role Not Found");
            if (role.Name == FSRoles.Admin)
            {
                throw new BadRequestException("Not allowed to modify Permissions for this role");
            }
            var currentPermission = await _roleManager.GetClaimsAsync(role);
            foreach(var per in currentPermission.Where(c => !request.Permissions.Any(x => x == c.Value)))
            {
                var result = await _roleManager.RemoveClaimAsync(role, per);
                if (!result.Succeeded)
                {
                    throw new InternalServerException("Update Permission failed");
                }
            }
            foreach (var per in request.Permissions.Where(x => !currentPermission.Any(c => c.Value == x)))
            {
                if (!string.IsNullOrEmpty(per))
                {
                    IdentityRoleClaim<Guid> claims = new IdentityRoleClaim<Guid>
                    {
                        RoleId = request.RoleId,
                        ClaimType = FSPermission.Permission,
                        ClaimValue = per
                    };
                    await _context.RoleClaims.AddAsync(claims);

                }

            }
            return "Permission Update Successful";
        }
    }
}
