using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record GetItemsQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<Item>>;

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, LENA.Application.Models.PagedResult<Item>>
    {
        private readonly IItemRepository _itemRepository;
        public GetItemsQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<LENA.Application.Models.PagedResult<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
            => await _itemRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
