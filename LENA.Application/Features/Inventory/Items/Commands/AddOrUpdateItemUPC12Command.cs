using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Commands
{
    public record AddOrUpdateItemUPC12Command(int ItemId, string UPC12) : IRequest;

    public class AddOrUpdateItemUPC12CommandHandler : IRequestHandler<AddOrUpdateItemUPC12Command>
    {
        private readonly IItemRepository _itemRepository;
        public AddOrUpdateItemUPC12CommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(AddOrUpdateItemUPC12Command request, CancellationToken cancellationToken)
            => await _itemRepository.AddOrUpdateUPC12Async(request.ItemId, request.UPC12);
    }
}
