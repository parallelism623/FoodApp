using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.Auth
{
    public class CurrentUserMiddleware : IMiddleware
    {
        private readonly ICurrentUserInitialize _currentUserInitialize;
        public CurrentUserMiddleware(ICurrentUserInitialize currentUserInitialize)
        {
            _currentUserInitialize = currentUserInitialize;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _currentUserInitialize.SetCurrentUser(context.User);
            await next(context);
        }
    }
}
