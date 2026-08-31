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
    public record SetItemFavoriteCommand(int ItemId, bool IsFavorite) : IRequest;

        public class SetItemFavoriteCommandHandler : IRequestHandler<SetItemFavoriteCommand>
        {
            private readonly IItemRepository _itemRepository;
            public SetItemFavoriteCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task Handle(SetItemFavoriteCommand request, CancellationToken cancellationToken)
                => await _itemRepository.SetFavoriteAsync(request.ItemId, request.IsFavorite);
        }
}