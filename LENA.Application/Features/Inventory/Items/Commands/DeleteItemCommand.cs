using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.Items.Commands
{
    public record DeleteItemCommand(int ItemId) : IRequest<Item?>;

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Item?>
    {
        private readonly IItemRepository _itemRepository;
        public DeleteItemCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<Item?> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(request.ItemId, cancellationToken) ?? throw new NotFoundException(nameof(Item), request.ItemId);

            return await _itemRepository.DeleteAsync(item, cancellationToken);
        }
    }
}