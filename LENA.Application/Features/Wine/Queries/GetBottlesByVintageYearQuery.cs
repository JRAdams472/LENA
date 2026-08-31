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
    public record GetBottlesByVintageYearQuery(int VintageYear) : IRequest<IReadOnlyList<Bottle>>;

        public class GetBottlesByVintageYearQueryHandler : IRequestHandler<GetBottlesByVintageYearQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetBottlesByVintageYearQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByVintageYearQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetAllByVintageYearAsync(request.VintageYear);
        }
}