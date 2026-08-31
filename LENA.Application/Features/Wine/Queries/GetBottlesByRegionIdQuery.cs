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
    public record GetBottlesByRegionIdQuery(int RegionId) : IRequest<IReadOnlyList<Bottle>>;

        public class GetBottlesByRegionIdQueryHandler : IRequestHandler<GetBottlesByRegionIdQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetBottlesByRegionIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByRegionIdQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetAllByRegionIdAsync(request.RegionId);
        }
}