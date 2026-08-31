using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottlesByTypeIdQuery(int TypeId) : IRequest<IReadOnlyList<Bottle>>;

        public class GetBottlesByTypeIdQueryHandler : IRequestHandler<GetBottlesByTypeIdQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetBottlesByTypeIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByTypeIdQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetAllByTypeIdAsync(request.TypeId, cancellationToken);
        }
}