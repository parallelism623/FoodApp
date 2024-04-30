using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.Caching;
using FoodShop.Contract.Abstraction.Message;
using MediatR;

namespace FoodShop.Application.Common.Behaviors
{
    public class CachingBehavior<TRequest, TRespone> : IPipelineBehavior<TRequest, TRespone>
    where TRequest : IRequest<TRespone>
    {
        private readonly ICacheServices _cacheServices;
        private readonly ICurrentUser _currentUser;
        public CachingBehavior(
            ICacheServices cacheServices,
            ICurrentUser currentUser)
        {
            _currentUser = currentUser;
            _cacheServices = cacheServices;
        }
        public async Task<TRespone> Handle(TRequest request, RequestHandlerDelegate<TRespone> next, CancellationToken cancellationToken)
        {
            if (!IsQuery(request))
            {
                return await next();
            }
            var userId = _currentUser.GetUserId().ToString() ?? string.Empty;
            var cacheKey = _cacheServices.GetCacheKey(userId, nameof(request), GetStringParams(request));
            var respone = await _cacheServices.GetCacheAsync<TRespone>(cacheKey, cancellationToken);
            if (respone is null)
            {
                respone = await next();
            }
            var slidingTime = TimeSpan.FromMinutes(10);
            await _cacheServices.SetCacheAsync(cacheKey, respone, slidingTime, cancellationToken);
            return respone;
        }
        private bool IsQuery(TRequest request)
            => nameof(request).EndsWith("Query");
        private string GetStringParams(TRequest request)
        {
            string result = "";
            foreach(var f in request.GetType().GetProperties())
            {
                result = result + nameof(f) + ":";
            }
            result.Remove(result.Length - 1);
            return result;
        }
    }
}
