using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Queries
{
    public record GetFoodNutrientsQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<FoodNutrient>>;

    public class GetFoodNutrientsQueryHandler : IRequestHandler<GetFoodNutrientsQuery, LENA.Application.Models.PagedResult<FoodNutrient>>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientsQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<LENA.Application.Models.PagedResult<FoodNutrient>> Handle(GetFoodNutrientsQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
