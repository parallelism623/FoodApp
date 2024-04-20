using FoodShop.Application.Services.DistributedCache;
using FoodShop.Contract.Abstraction.Message;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Behaviors
{
    public class CachingBehavior<TRequest, TRespone> : IPipelineBehavior<TRequest, TRespone>
    where TRequest : IRequest<TRespone>, ICachedQuery
    {
        private readonly ICacheServices _cacheServices; 
        public CachingBehavior(ICacheServices cacheServices)
        {
            _cacheServices = cacheServices;
        }
        public async Task<TRespone> Handle(TRequest request, RequestHandlerDelegate<TRespone> next, CancellationToken cancellationToken)
        {
            if (!IsQuery(request))
            {
                return await next();
            }
            
            var respone = await _cacheServices.GetCacheAsync<TRespone>(request.cacheKey, cancellationToken);
            if (respone is null)
            {
                respone = await next();
            }
            var slidingTime = TimeSpan.FromMinutes(10);
            await _cacheServices.SetCacheAsync<TRespone>(request.cacheKey, respone, slidingTime, cancellationToken);
            return respone;
        }
        private bool IsQuery(TRequest request)
            => nameof(request).EndsWith("Query");

    }
}
