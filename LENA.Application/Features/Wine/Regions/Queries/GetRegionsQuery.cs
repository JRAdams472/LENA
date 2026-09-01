using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Regions.Queries
{
    public record GetRegionsQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<Region>>;

    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, LENA.Application.Models.PagedResult<Region>>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionsQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<LENA.Application.Models.PagedResult<Region>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
            => await _regionRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
