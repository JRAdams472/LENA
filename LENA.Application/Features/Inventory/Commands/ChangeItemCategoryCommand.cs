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
    public record ChangeItemCategoryCommand(int ItemId, int NewCategoryId) : IRequest;

        public class ChangeItemCategoryCommandHandler : IRequestHandler<ChangeItemCategoryCommand>
        {
            private readonly IItemRepository _itemRepository;
            public ChangeItemCategoryCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task Handle(ChangeItemCategoryCommand request, CancellationToken cancellationToken)
                => await _itemRepository.ChangeItemCategoryAsync(request.ItemId, request.NewCategoryId);
        }
}