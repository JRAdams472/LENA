using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Queries
{
    public record GetFoodNutrientsByFoodIdQuery(int FoodId) : IRequest<IEnumerable<FoodNutrient>>;

    public class GetFoodNutrientsByFoodIdQueryHandler : IRequestHandler<GetFoodNutrientsByFoodIdQuery, IEnumerable<FoodNutrient>>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientsByFoodIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<IEnumerable<FoodNutrient>> Handle(GetFoodNutrientsByFoodIdQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.GetByFoodIdAsync(request.FoodId, cancellationToken);
    }
}
