using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Countries.Commands
{
    public record UpdateCountryCommand(Country Country) : IRequest<Country>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => Country;
    }

    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Country>
    {
        private readonly ICountryRepository _countryRepository;

        private readonly IMemoryCache _cache;

        public UpdateCountryCommandHandler(ICountryRepository countryRepository, IMemoryCache cache)
        {
            _countryRepository = countryRepository;
            _cache = cache;
        }

        public async Task<Country> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var result = await _countryRepository.UpdateAsync(request.Country, cancellationToken);
            _cache.Remove(CacheKeys.Countries);
            _cache.Remove(CacheKeys.ActiveCountries);
            return result;
        }
    }
}