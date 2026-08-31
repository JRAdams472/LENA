using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine.Queries
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
            return await _countryRepository.GetByIdAsync(request.CountryId);
        }
    }
}
