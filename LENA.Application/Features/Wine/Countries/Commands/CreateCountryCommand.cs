using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Countries.Commands
{
    public record CreateCountryCommand(Country Country) : IRequest<Country>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Country;
    }

    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Country>
    {
        private readonly ICountryRepository _countryRepository;

        private readonly IMemoryCache _cache;

        public CreateCountryCommandHandler(ICountryRepository countryRepository, IMemoryCache cache)
        {
            _countryRepository = countryRepository;
            _cache = cache;
        }

        public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var result = await _countryRepository.CreateAsync(request.Country, cancellationToken);
            _cache.Remove(CacheKeys.Countries);
            _cache.Remove(CacheKeys.ActiveCountries);
            return result;
        }
    }
}