using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace LENA.Application.Features.Wine.Queries
{
    public record GetRegionsByCountryIdQuery(int CountryId) : IRequest<IReadOnlyList<Region>>;

        public class GetRegionsByCountryIdQueryHandler : IRequestHandler<GetRegionsByCountryIdQuery, IReadOnlyList<Region>>
        {
            private readonly IRegionRepository _regionRepository;
            public GetRegionsByCountryIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
            public async Task<IReadOnlyList<Region>> Handle(GetRegionsByCountryIdQuery request, CancellationToken cancellationToken)
                => await _regionRepository.GetAllByCountryIdAsync(request.CountryId);
        }
}