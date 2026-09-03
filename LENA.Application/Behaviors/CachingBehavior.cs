using System;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Caching;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly IMemoryCache _cache;

        public CachingBehavior(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ICacheableQuery<TResponse> cacheable)
            {
                return await next();
            }

            if (_cache.TryGetValue(cacheable.CacheKey, out TResponse? cached) && cached is not null)
            {
                return cached;
            }

            var result = await next();
            _cache.Set(cacheable.CacheKey, result, cacheable.CacheDuration);
            return result;
        }
    }
}