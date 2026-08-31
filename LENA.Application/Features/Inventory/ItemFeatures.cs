using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Inventory
{
    // Queries
    public record GetItemByIdQuery(int ItemId) : IRequest<Item?>;
    public record GetItemsQuery : IRequest<IReadOnlyList<Item>>;
    public record GetItemByNameQuery(string Name) : IRequest<Item?>;

    // Commands
    public record CreateItemCommand(Item Item) : IRequest<Item>;
    public record UpdateItemCommand(Item Item) : IRequest<Item>;
    public record DeleteItemCommand(int ItemId) : IRequest<Item?>;

    public record ChangeItemCategoryCommand(int ItemId, int NewCategoryId) : IRequest;
    public record AddOrUpdateItemUPC12Command(int ItemId, string UPC12) : IRequest;
    public record AddOrUpdateItemUPC14Command(int ItemId, string UPC14) : IRequest;
    public record AdjustItemQuantityCommand(int ItemId, decimal Quantity, DateTime? PurchaseDate) : IRequest;
    public record SetItemFavoriteCommand(int ItemId, bool IsFavorite) : IRequest;

    // Handlers
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item?>
    {
        private readonly IItemRepository _itemRepository;
        public GetItemByIdQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<Item?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
            => await _itemRepository.GetByIdAsync(request.ItemId);
    }

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IReadOnlyList<Item>>
    {
        private readonly IItemRepository _itemRepository;
        public GetItemsQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<IReadOnlyList<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
            => await _itemRepository.ListAllAsync();
    }

    public class GetItemByNameQueryHandler : IRequestHandler<GetItemByNameQuery, Item?>
    {
        private readonly IItemRepository _itemRepository;
        public GetItemByNameQueryHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<Item?> Handle(GetItemByNameQuery request, CancellationToken cancellationToken)
            => await _itemRepository.GetByNameAsync(request.Name);
    }

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Item>
    {
        private readonly IItemRepository _itemRepository;
        public CreateItemCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<Item> Handle(CreateItemCommand request, CancellationToken cancellationToken)
            => await _itemRepository.CreateAsync(request.Item);
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Item>
    {
        private readonly IItemRepository _itemRepository;
        public UpdateItemCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task<Item> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
            => await _itemRepository.UpdateAsync(request.Item);
    }

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

    public class ChangeItemCategoryCommandHandler : IRequestHandler<ChangeItemCategoryCommand>
    {
        private readonly IItemRepository _itemRepository;
        public ChangeItemCategoryCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(ChangeItemCategoryCommand request, CancellationToken cancellationToken)
            => await _itemRepository.ChangeItemCategoryAsync(request.ItemId, request.NewCategoryId);
    }

    public class AddOrUpdateItemUPC12CommandHandler : IRequestHandler<AddOrUpdateItemUPC12Command>
    {
        private readonly IItemRepository _itemRepository;
        public AddOrUpdateItemUPC12CommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(AddOrUpdateItemUPC12Command request, CancellationToken cancellationToken)
            => await _itemRepository.AddOrUpdateUPC12Async(request.ItemId, request.UPC12);
    }

    public class AddOrUpdateItemUPC14CommandHandler : IRequestHandler<AddOrUpdateItemUPC14Command>
    {
        private readonly IItemRepository _itemRepository;
        public AddOrUpdateItemUPC14CommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(AddOrUpdateItemUPC14Command request, CancellationToken cancellationToken)
            => await _itemRepository.AddOrUpdateUPC14Async(request.ItemId, request.UPC14);
    }

    public class AdjustItemQuantityCommandHandler : IRequestHandler<AdjustItemQuantityCommand>
    {
        private readonly IItemRepository _itemRepository;
        public AdjustItemQuantityCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(AdjustItemQuantityCommand request, CancellationToken cancellationToken)
            => await _itemRepository.AdjustQuantityAsync(request.ItemId, request.Quantity, request.PurchaseDate);
    }

    public class SetItemFavoriteCommandHandler : IRequestHandler<SetItemFavoriteCommand>
    {
        private readonly IItemRepository _itemRepository;
        public SetItemFavoriteCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(SetItemFavoriteCommand request, CancellationToken cancellationToken)
            => await _itemRepository.SetFavoriteAsync(request.ItemId, request.IsFavorite);
    }
}
