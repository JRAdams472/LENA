using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Regions.Queries
{
    public record GetRegionsQuery : IRequest<IReadOnlyList<Region>>, ICacheableQuery<IReadOnlyList<Region>>
    {
        public string CacheKey => CacheKeys.Regions;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(10);
    }

    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, IReadOnlyList<Region>>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionsQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<IReadOnlyList<Region>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
            => await _regionRepository.ListAllAsync(cancellationToken);
    }
}