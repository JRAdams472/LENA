using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Commands
{
    public record DeleteGroceryListItemCommand(int GroceryListItemId) : IRequest<Unit>;

    public class DeleteGroceryListItemCommandHandler : IRequestHandler<DeleteGroceryListItemCommand, Unit>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public DeleteGroceryListItemCommandHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<Unit> Handle(DeleteGroceryListItemCommand request, CancellationToken cancellationToken)
        {
            await _groceryListRepository.DeleteGroceryListItemAsync(request.GroceryListItemId, cancellationToken);
            return Unit.Value;
        }
    }
}
