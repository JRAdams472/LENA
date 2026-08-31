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
    public record GetActiveVintagesQuery : IRequest<IReadOnlyList<Vintage>>;

        public class GetActiveVintagesQueryHandler : IRequestHandler<GetActiveVintagesQuery, IReadOnlyList<Vintage>>
        {
            private readonly IVintageRepository _vintageRepository;
            public GetActiveVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
            public async Task<IReadOnlyList<Vintage>> Handle(GetActiveVintagesQuery request, CancellationToken cancellationToken)
                => await _vintageRepository.GetAllActiveAsync();
        }
}