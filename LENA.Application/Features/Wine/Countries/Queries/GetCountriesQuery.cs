using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Countries.Queries
{
    public record GetCountriesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<Country>>;

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, LENA.Application.Models.PagedResult<Country>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<LENA.Application.Models.PagedResult<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _countryRepository.ListAllAsync(request.Paging, cancellationToken);
        }
    }
}
