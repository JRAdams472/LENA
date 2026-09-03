using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Countries.Commands
{
    public record DeleteCountryCommand(int CountryId) : IRequest<Country?>;

    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Country?>
    {
        private readonly ICountryRepository _countryRepository;

        private readonly IMemoryCache _cache;

        public DeleteCountryCommandHandler(ICountryRepository countryRepository, IMemoryCache cache)
        {
            _countryRepository = countryRepository;
            _cache = cache;
        }

        public async Task<Country?> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken) ?? throw new NotFoundException(nameof(Country), request.CountryId);

            var result = await _countryRepository.DeleteAsync(country, cancellationToken);
            _cache.Remove(CacheKeys.Countries);
            _cache.Remove(CacheKeys.ActiveCountries);
            return result;
        }
    }
}