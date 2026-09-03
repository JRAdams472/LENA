using MediatR;

namespace LENA.Application.Contracts.Caching
{
    public interface ICacheableQuery<TResponse> : IRequest<TResponse>
    {
        string CacheKey { get; }

        TimeSpan CacheDuration { get; }
    }
}