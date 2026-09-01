using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Grocery;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Queries
{
    public record GetGroceryListsQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<GroceryList>>;

    public class GetGroceryListsQueryHandler : IRequestHandler<GetGroceryListsQuery, LENA.Application.Models.PagedResult<GroceryList>>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public GetGroceryListsQueryHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<LENA.Application.Models.PagedResult<GroceryList>> Handle(GetGroceryListsQuery request, CancellationToken cancellationToken)
        {
            return await _groceryListRepository.ListAllAsync(request.Paging, cancellationToken);
        }
    }
}
