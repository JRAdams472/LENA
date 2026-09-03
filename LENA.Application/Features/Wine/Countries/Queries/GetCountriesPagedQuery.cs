using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Countries.Queries
{
    public record GetCountriesPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<Country>>;

    public class GetCountriesPagedQueryHandler : IRequestHandler<GetCountriesPagedQuery, LENA.Application.Models.PagedResult<Country>>
    {
        private readonly ICountryRepository _countryRepository;
        public GetCountriesPagedQueryHandler(ICountryRepository countryRepository) => _countryRepository = countryRepository;
        public async Task<LENA.Application.Models.PagedResult<Country>> Handle(GetCountriesPagedQuery request, CancellationToken cancellationToken)
            => await _countryRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}