using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record GetItemsQuery : IRequest<IReadOnlyList<Item>>;

        public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IReadOnlyList<Item>>
        {
            private readonly IItemRepository _itemRepository;
            public GetItemsQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task<IReadOnlyList<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
                => await _itemRepository.ListAllAsync();
        }
}