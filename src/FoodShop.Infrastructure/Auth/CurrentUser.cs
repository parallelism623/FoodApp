using FoodShop.Application.Common.Auth;
using FoodShop.Infrastructure.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.Auth
{
    public class CurrentUser : ICurrentUserInitialize, ICurrentUser
    {
        private ClaimsPrincipal? _user;
        private Guid _userId = Guid.Empty;
        public string? Name => _user?.Identity?.Name;



        public IEnumerable<Claim>? GetUserClaims() => _user?.Claims;

        public string? GetUserEmail() => IsAuthenticated() ? _user!.GetId() : string.Empty;

        public Guid GetUserId() => IsAuthenticated() ? Guid.Parse(_user.GetId() ?? Guid.Empty.ToString()) : _userId;

        public bool IsAuthenticated() => _user?.Identity?.IsAuthenticated is true;

        public bool IsInRole(string role) => _user?.IsInRole(role) is true;

        public void SetCurrentUser(ClaimsPrincipal user)
        {
            if (_user != null)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            _user = user;
        }


        public void SetCurrentUserId(string userId)
        {
            if (_userId != Guid.Empty)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            if (!string.IsNullOrEmpty(userId))
            {
                _userId = Guid.Parse(userId);
            }
        }
    }
}
