using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Grocery;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Commands
{
    public record ToggleGroceryListItemCheckedCommand(int GroceryListItemId) : IRequest<GroceryListItem>, IUpdateCommand
    {
        private readonly GroceryListItem _groceryListItem = new() { GroceryListItemID = GroceryListItemId };

        public AuditableEntity AuditableEntity => _groceryListItem;
    }

    public class ToggleGroceryListItemCheckedCommandHandler : IRequestHandler<ToggleGroceryListItemCheckedCommand, GroceryListItem>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public ToggleGroceryListItemCheckedCommandHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<GroceryListItem> Handle(ToggleGroceryListItemCheckedCommand request, CancellationToken cancellationToken)
        {
            var groceryListItem = (GroceryListItem)request.AuditableEntity;
            return await _groceryListRepository.ToggleGroceryListItemCheckedAsync(groceryListItem, cancellationToken);
        }
    }
}
