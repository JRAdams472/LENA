using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Countries.Queries
{
    public record GetCountriesQuery : IRequest<IReadOnlyList<Country>>, ICacheableQuery<IReadOnlyList<Country>>
    {
        public string CacheKey => CacheKeys.Countries;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(10);
    }

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IReadOnlyList<Country>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IReadOnlyList<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _countryRepository.ListAllAsync(cancellationToken);
        }
    }
}