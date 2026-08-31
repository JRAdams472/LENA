using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottlesByCountryIdQuery(int CountryId) : IRequest<IReadOnlyList<Bottle>>;

        public class GetBottlesByCountryIdQueryHandler : IRequestHandler<GetBottlesByCountryIdQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetBottlesByCountryIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByCountryIdQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetAllByCountryIdAsync(request.CountryId);
        }
}