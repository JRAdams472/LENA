using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Commands
{
    public record UpdateItemCommand(Item Item) : IRequest<Item>;

        public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Item>
        {
            private readonly IItemRepository _itemRepository;
            public UpdateItemCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task<Item> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
                => await _itemRepository.UpdateAsync(request.Item);
        }
}