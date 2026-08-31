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
    public record GetFavoriteBottlesQuery : IRequest<IReadOnlyList<Bottle>>;

        public class GetFavoriteBottlesQueryHandler : IRequestHandler<GetFavoriteBottlesQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetFavoriteBottlesQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetFavoriteBottlesQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetFavoritesAsync();
        }
}