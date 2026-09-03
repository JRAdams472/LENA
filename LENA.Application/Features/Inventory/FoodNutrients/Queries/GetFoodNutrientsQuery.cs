using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Queries
{
    public record GetFoodNutrientsQuery : IRequest<IReadOnlyList<FoodNutrient>>;

    public class GetFoodNutrientsQueryHandler : IRequestHandler<GetFoodNutrientsQuery, IReadOnlyList<FoodNutrient>>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientsQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<IReadOnlyList<FoodNutrient>> Handle(GetFoodNutrientsQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.ListAllAsync(cancellationToken);
    }
}