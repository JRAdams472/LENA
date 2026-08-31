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
    public record GetVintagesQuery : IRequest<IReadOnlyList<Vintage>>;

        public class GetVintagesQueryHandler : IRequestHandler<GetVintagesQuery, IReadOnlyList<Vintage>>
        {
            private readonly IVintageRepository _vintageRepository;
            public GetVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
            public async Task<IReadOnlyList<Vintage>> Handle(GetVintagesQuery request, CancellationToken cancellationToken)
                => await _vintageRepository.ListAllAsync();
        }
}