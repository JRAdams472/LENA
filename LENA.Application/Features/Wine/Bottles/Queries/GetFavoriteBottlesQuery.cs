using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetFavoriteBottlesQuery : IRequest<IReadOnlyList<Bottle>>;

        public class GetFavoriteBottlesQueryHandler : IRequestHandler<GetFavoriteBottlesQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetFavoriteBottlesQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetFavoriteBottlesQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetFavoritesAsync(cancellationToken);
        }
}