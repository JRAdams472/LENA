using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine.Queries
{
    public record GetCountryByISOCodeQuery(string ISOCode) : IRequest<Country?>;
    public record GetActiveCountriesQuery : IRequest<IReadOnlyList<Country>>;

    public class GetCountryByISOCodeQueryHandler : IRequestHandler<GetCountryByISOCodeQuery, Country?>
    {
        private readonly ICountryRepository _countryRepository;
        public GetCountryByISOCodeQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
        public async Task<Country?> Handle(GetCountryByISOCodeQuery request, CancellationToken cancellationToken)
            => await _countryRepository.GetByISOCodeAsync(request.ISOCode);
    }

    public class GetActiveCountriesQueryHandler : IRequestHandler<GetActiveCountriesQuery, IReadOnlyList<Country>>
    {
        private readonly ICountryRepository _countryRepository;
        public GetActiveCountriesQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
        public async Task<IReadOnlyList<Country>> Handle(GetActiveCountriesQuery request, CancellationToken cancellationToken)
            => await _countryRepository.GetAllActiveAsync();
    }
}
