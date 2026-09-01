using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottlesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<Bottle>>;

    public class GetBottlesQueryHandler : IRequestHandler<GetBottlesQuery, LENA.Application.Models.PagedResult<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;

        public GetBottlesQueryHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<LENA.Application.Models.PagedResult<Bottle>> Handle(GetBottlesQuery request, CancellationToken cancellationToken)
        {
            return await _bottleRepository.ListAllAsync(request.Paging, cancellationToken);
        }
    }
}
