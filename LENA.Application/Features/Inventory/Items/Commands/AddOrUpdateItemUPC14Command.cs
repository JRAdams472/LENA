using LENA.Application.Contracts.Persistence;

using MediatR;

namespace LENA.Application.Features.Inventory.Items.Commands
{
    public record AddOrUpdateItemUPC14Command(int ItemId, string UPC14) : IRequest;

    public class AddOrUpdateItemUPC14CommandHandler : IRequestHandler<AddOrUpdateItemUPC14Command>
    {
        private readonly IItemRepository _itemRepository;
        public AddOrUpdateItemUPC14CommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(AddOrUpdateItemUPC14Command request, CancellationToken cancellationToken)
            => await _itemRepository.AddOrUpdateUPC14Async(request.ItemId, request.UPC14);
    }
}