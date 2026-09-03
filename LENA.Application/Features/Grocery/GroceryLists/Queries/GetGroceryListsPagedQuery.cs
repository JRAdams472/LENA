using System.Linq;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Models;
using LENA.Domain.Entity.Grocery;

using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Queries
{
    public record GetGroceryListsPagedQuery(int PageNumber, int PageSize) : IRequest<PagedResult<GroceryList>>;

    public class GetGroceryListsPagedQueryHandler : IRequestHandler<GetGroceryListsPagedQuery, PagedResult<GroceryList>>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public GetGroceryListsPagedQueryHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<PagedResult<GroceryList>> Handle(GetGroceryListsPagedQuery request, CancellationToken cancellationToken)
        {
            var (pageNumber, pageSize) = PaginationRequest.Clamp(request.PageNumber, request.PageSize);
            var all = await _groceryListRepository.ListAllAsync(cancellationToken);
            var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<GroceryList>
            {
                Items = items,
                TotalCount = all.Count,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
    }
}