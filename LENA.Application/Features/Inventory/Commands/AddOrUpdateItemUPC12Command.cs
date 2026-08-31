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
    public record AddOrUpdateItemUPC12Command(int ItemId, string UPC12) : IRequest;

        public class AddOrUpdateItemUPC12CommandHandler : IRequestHandler<AddOrUpdateItemUPC12Command>
        {
            private readonly IItemRepository _itemRepository;
            public AddOrUpdateItemUPC12CommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task Handle(AddOrUpdateItemUPC12Command request, CancellationToken cancellationToken)
                => await _itemRepository.AddOrUpdateUPC12Async(request.ItemId, request.UPC12);
        }
}