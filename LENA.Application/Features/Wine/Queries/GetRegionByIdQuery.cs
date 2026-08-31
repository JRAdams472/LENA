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
    public record GetRegionByIdQuery(int RegionId) : IRequest<Region?>;

        public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, Region?>
        {
            private readonly IRegionRepository _regionRepository;
            public GetRegionByIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
            public async Task<Region?> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
                => await _regionRepository.GetByIdAsync(request.RegionId);
        }
}