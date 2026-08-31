using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Countries.Queries
{
    public record GetCountryByISOCodeQuery(string ISOCode) : IRequest<Country?>;

    public class GetCountryByISOCodeQueryHandler : IRequestHandler<GetCountryByISOCodeQuery, Country?>
    {
        private readonly ICountryRepository _countryRepository;
        public GetCountryByISOCodeQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
        public async Task<Country?> Handle(GetCountryByISOCodeQuery request, CancellationToken cancellationToken)
            => await _countryRepository.GetByISOCodeAsync(request.ISOCode, cancellationToken);
    }
}
