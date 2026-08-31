using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine.Queries
{
    public record GetCountriesQuery : IRequest<IReadOnlyList<Country>>;

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IReadOnlyList<Country>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IReadOnlyList<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _countryRepository.ListAllAsync();
        }
    }
}
