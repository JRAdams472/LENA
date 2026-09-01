using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Queries
{
    public record GetFoodFlavorsQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<FoodFlavor>>;

    public class GetFoodFlavorsQueryHandler : IRequestHandler<GetFoodFlavorsQuery, LENA.Application.Models.PagedResult<FoodFlavor>>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorsQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<LENA.Application.Models.PagedResult<FoodFlavor>> Handle(GetFoodFlavorsQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
