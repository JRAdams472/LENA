using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Countries.Queries
{
    public record GetCountryByIdQuery(int CountryId) : IRequest<Country?>;

    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Country?>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryByIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken) ?? throw new NotFoundException(nameof(Country), request.CountryId);
        }
    }
}