using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Commands
{
    public record SetItemFavoriteCommand(int ItemId, bool IsFavorite) : IRequest;

        public class SetItemFavoriteCommandHandler : IRequestHandler<SetItemFavoriteCommand>
        {
            private readonly IItemRepository _itemRepository;
            public SetItemFavoriteCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
            public async Task Handle(SetItemFavoriteCommand request, CancellationToken cancellationToken)
                => await _itemRepository.SetFavoriteAsync(request.ItemId, request.IsFavorite, cancellationToken);
        }
}