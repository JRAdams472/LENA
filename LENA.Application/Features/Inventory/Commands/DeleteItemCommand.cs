using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LENA.Application.Features.Inventory.Commands
{
    public record DeleteItemCommand(int ItemId) : IRequest<Item?>;

        public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Item?>
        {
            private readonly IItemRepository _itemRepository;
            public DeleteItemCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task<Item?> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
            {
                var item = await _itemRepository.GetByIdAsync(request.ItemId);
                if (item == null)
                    return null;
    
                return await _itemRepository.DeleteAsync(item);
            }
        }
}