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
    public record GetActiveCountriesQuery : IRequest<IReadOnlyList<Country>>;

        public class GetActiveCountriesQueryHandler : IRequestHandler<GetActiveCountriesQuery, IReadOnlyList<Country>>
        {
            private readonly ICountryRepository _countryRepository;
            public GetActiveCountriesQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
            public async Task<IReadOnlyList<Country>> Handle(GetActiveCountriesQuery request, CancellationToken cancellationToken)
                => await _countryRepository.GetAllActiveAsync();
        }
}