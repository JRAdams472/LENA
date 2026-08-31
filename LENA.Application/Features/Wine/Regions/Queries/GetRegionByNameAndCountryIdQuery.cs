using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Regions.Queries
{
    public record GetRegionByNameAndCountryIdQuery(string Name, int CountryId) : IRequest<Region?>;

        public class GetRegionByNameAndCountryIdQueryHandler : IRequestHandler<GetRegionByNameAndCountryIdQuery, Region?>
        {
            private readonly IRegionRepository _regionRepository;
            public GetRegionByNameAndCountryIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
            public async Task<Region?> Handle(GetRegionByNameAndCountryIdQuery request, CancellationToken cancellationToken)
                => await _regionRepository.GetByNameAndCountryIdAsync(request.Name, request.CountryId);
        }
}