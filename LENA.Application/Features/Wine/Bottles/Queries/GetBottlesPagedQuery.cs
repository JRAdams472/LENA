using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottlesPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<Bottle>>;

    public class GetBottlesPagedQueryHandler : IRequestHandler<GetBottlesPagedQuery, LENA.Application.Models.PagedResult<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetBottlesPagedQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<LENA.Application.Models.PagedResult<Bottle>> Handle(GetBottlesPagedQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
