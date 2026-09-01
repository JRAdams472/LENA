using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Grocery;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Commands
{
    public record AddGroceryListItemCommand(GroceryListItem GroceryListItem) : IRequest<GroceryListItem>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => GroceryListItem;
    }

    public class AddGroceryListItemCommandHandler : IRequestHandler<AddGroceryListItemCommand, GroceryListItem>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public AddGroceryListItemCommandHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<GroceryListItem> Handle(AddGroceryListItemCommand request, CancellationToken cancellationToken)
        {
            return await _groceryListRepository.AddGroceryListItemAsync(request.GroceryListItem, cancellationToken);
        }
    }
}
