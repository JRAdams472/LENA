using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Grocery;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Queries
{
    public record GetGroceryListByIdQuery(int GroceryListId) : IRequest<GroceryList?>;

    public class GetGroceryListByIdQueryHandler : IRequestHandler<GetGroceryListByIdQuery, GroceryList?>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public GetGroceryListByIdQueryHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<GroceryList?> Handle(GetGroceryListByIdQuery request, CancellationToken cancellationToken)
        {
            return await _groceryListRepository.GetByIdAsync(request.GroceryListId, cancellationToken) ?? throw new NotFoundException(nameof(GroceryList), request.GroceryListId);
        }
    }
}
