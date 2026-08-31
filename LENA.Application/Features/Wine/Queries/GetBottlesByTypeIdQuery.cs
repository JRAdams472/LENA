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
    public record GetBottlesByTypeIdQuery(int TypeId) : IRequest<IReadOnlyList<Bottle>>;

        public class GetBottlesByTypeIdQueryHandler : IRequestHandler<GetBottlesByTypeIdQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetBottlesByTypeIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByTypeIdQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetAllByTypeIdAsync(request.TypeId);
        }
}