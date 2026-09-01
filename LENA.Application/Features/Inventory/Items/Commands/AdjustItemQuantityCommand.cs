using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using MediatR;

namespace LENA.Application.Features.Inventory.Items.Commands
{
    public record AdjustItemQuantityCommand(int ItemId, decimal Quantity, DateTime? PurchaseDate) : IRequest, IUpdateCommand
    {
        private readonly AuditableEntity _audit = new();

        public AuditableEntity AuditableEntity => _audit;
    }

    public class AdjustItemQuantityCommandHandler : IRequestHandler<AdjustItemQuantityCommand>
    {
        private readonly IItemRepository _itemRepository;
        public AdjustItemQuantityCommandHandler(IItemRepository itemRepository) => _itemRepository = itemRepository;
        public async Task Handle(AdjustItemQuantityCommand request, CancellationToken cancellationToken)
            => await _itemRepository.AdjustQuantityAsync(
                request.ItemId,
                request.Quantity,
                request.PurchaseDate,
                request.AuditableEntity.LastUpdatedBy,
                cancellationToken);
    }
}
