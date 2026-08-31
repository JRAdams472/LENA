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
    public record GetTotalBottleCountQuery : IRequest<int>;

        public class GetTotalBottleCountQueryHandler : IRequestHandler<GetTotalBottleCountQuery, int>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetTotalBottleCountQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<int> Handle(GetTotalBottleCountQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetTotalBottleCountAsync();
        }
}