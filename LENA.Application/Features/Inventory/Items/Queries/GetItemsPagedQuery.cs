using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record GetItemsPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<Item>>;

    public class GetItemsPagedQueryHandler : IRequestHandler<GetItemsPagedQuery, LENA.Application.Models.PagedResult<Item>>
    {
        private readonly IItemRepository _itemRepository;
        public GetItemsPagedQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<LENA.Application.Models.PagedResult<Item>> Handle(GetItemsPagedQuery request, CancellationToken cancellationToken)
            => await _itemRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
