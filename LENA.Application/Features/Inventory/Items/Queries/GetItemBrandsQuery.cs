using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Queries
{
    public record GetItemBrandsQuery(string? Search) : IRequest<IReadOnlyList<string>>;

    public class GetItemBrandsQueryHandler : IRequestHandler<GetItemBrandsQuery, IReadOnlyList<string>>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemBrandsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IReadOnlyList<string>> Handle(GetItemBrandsQuery request, CancellationToken cancellationToken)
            => await _itemRepository.GetBrandsAsync(request.Search, cancellationToken);
    }
}
