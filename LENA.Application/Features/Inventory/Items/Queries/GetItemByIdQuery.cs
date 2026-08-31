using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record GetItemByIdQuery(int ItemId) : IRequest<Item?>;

        public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item?>
        {
            private readonly IItemRepository _itemRepository;
            public GetItemByIdQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task<Item?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
                => await _itemRepository.GetByIdAsync(request.ItemId);
        }
}