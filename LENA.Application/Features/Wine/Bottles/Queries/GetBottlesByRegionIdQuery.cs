using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
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