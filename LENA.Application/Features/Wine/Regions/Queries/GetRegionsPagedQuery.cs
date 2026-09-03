using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Regions.Queries
{
    public record GetRegionsPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<Region>>;

    public class GetRegionsPagedQueryHandler : IRequestHandler<GetRegionsPagedQuery, LENA.Application.Models.PagedResult<Region>>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionsPagedQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<LENA.Application.Models.PagedResult<Region>> Handle(GetRegionsPagedQuery request, CancellationToken cancellationToken)
            => await _regionRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}