using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Queries
{
    public record GetVintagesPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<Vintage>>;

    public class GetVintagesPagedQueryHandler : IRequestHandler<GetVintagesPagedQuery, LENA.Application.Models.PagedResult<Vintage>>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintagesPagedQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<LENA.Application.Models.PagedResult<Vintage>> Handle(GetVintagesPagedQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}