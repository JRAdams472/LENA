using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Grocery;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Queries
{
    public record GetGroceryListsQuery : IRequest<IReadOnlyList<GroceryList>>;

    public class GetGroceryListsQueryHandler : IRequestHandler<GetGroceryListsQuery, IReadOnlyList<GroceryList>>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public GetGroceryListsQueryHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<IReadOnlyList<GroceryList>> Handle(GetGroceryListsQuery request, CancellationToken cancellationToken)
        {
            return await _groceryListRepository.ListAllAsync(cancellationToken);
        }
    }
}
