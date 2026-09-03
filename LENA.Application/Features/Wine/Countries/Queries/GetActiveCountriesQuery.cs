using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Countries.Queries
{
    public record GetActiveCountriesQuery : IRequest<IReadOnlyList<Country>>, ICacheableQuery<IReadOnlyList<Country>>
    {
        public string CacheKey => CacheKeys.ActiveCountries;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(10);
    }

    public class GetActiveCountriesQueryHandler : IRequestHandler<GetActiveCountriesQuery, IReadOnlyList<Country>>
    {
        private readonly ICountryRepository _countryRepository;
        public GetActiveCountriesQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
        public async Task<IReadOnlyList<Country>> Handle(GetActiveCountriesQuery request, CancellationToken cancellationToken)
            => await _countryRepository.GetAllActiveAsync(cancellationToken);
    }
}