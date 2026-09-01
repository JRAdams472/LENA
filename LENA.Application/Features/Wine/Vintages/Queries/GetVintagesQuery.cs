using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Queries
{
    public record GetVintagesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<Vintage>>;

    public class GetVintagesQueryHandler : IRequestHandler<GetVintagesQuery, LENA.Application.Models.PagedResult<Vintage>>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<LENA.Application.Models.PagedResult<Vintage>> Handle(GetVintagesQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
