using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Regions.Queries
{
    public record GetRegionByIdQuery(int RegionId) : IRequest<Region?>;

        public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, Region?>
        {
            private readonly IRegionRepository _regionRepository;
            public GetRegionByIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
            public async Task<Region?> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
                => await _regionRepository.GetByIdAsync(request.RegionId);
        }
}