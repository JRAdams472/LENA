using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record SearchItemsQuery(string Search, string? Brand = null, int Limit = 50) : IRequest<IReadOnlyList<Item>>;

    public class SearchItemsQueryHandler : IRequestHandler<SearchItemsQuery, IReadOnlyList<Item>>
    {
        private readonly IItemRepository _itemRepository;

        public SearchItemsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IReadOnlyList<Item>> Handle(SearchItemsQuery request, CancellationToken cancellationToken)
            => await _itemRepository.SearchAsync(request.Search, request.Brand, request.Limit, cancellationToken);
    }
}