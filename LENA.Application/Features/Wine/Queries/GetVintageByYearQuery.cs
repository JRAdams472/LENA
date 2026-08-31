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
    public record GetVintageByYearQuery(int Year) : IRequest<Vintage?>;

        public class GetVintageByYearQueryHandler : IRequestHandler<GetVintageByYearQuery, Vintage?>
        {
            private readonly IVintageRepository _vintageRepository;
            public GetVintageByYearQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
            public async Task<Vintage?> Handle(GetVintageByYearQuery request, CancellationToken cancellationToken)
                => await _vintageRepository.GetByYearAsync(request.Year);
        }
}