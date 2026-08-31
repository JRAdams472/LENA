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
    public record GetCountryByISOCodeQuery(string ISOCode) : IRequest<Country?>;

        public class GetCountryByISOCodeQueryHandler : IRequestHandler<GetCountryByISOCodeQuery, Country?>
        {
            private readonly ICountryRepository _countryRepository;
            public GetCountryByISOCodeQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
            public async Task<Country?> Handle(GetCountryByISOCodeQuery request, CancellationToken cancellationToken)
                => await _countryRepository.GetByISOCodeAsync(request.ISOCode);
        }
}