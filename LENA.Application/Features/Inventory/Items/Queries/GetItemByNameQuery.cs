using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record GetItemByNameQuery(string Name) : IRequest<Item?>;

        public class GetItemByNameQueryHandler : IRequestHandler<GetItemByNameQuery, Item?>
        {
            private readonly IItemRepository _itemRepository;
            public GetItemByNameQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task<Item?> Handle(GetItemByNameQuery request, CancellationToken cancellationToken)
                => await _itemRepository.GetByNameAsync(request.Name);
        }
}