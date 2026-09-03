using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.Items.Commands
{
    public record CreateItemCommand(Item Item) : IRequest<Item>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Item;
    }

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Item>
    {
        private readonly IItemRepository _itemRepository;
        public CreateItemCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<Item> Handle(CreateItemCommand request, CancellationToken cancellationToken)
            => await _itemRepository.CreateAsync(request.Item, cancellationToken);
    }
}