using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Queries
{
    public record GetFoodNutrientByFoodAndNutrientIdQuery(int FoodId, int NutrientId) : IRequest<FoodNutrient?>;

        public class GetFoodNutrientByFoodAndNutrientIdQueryHandler : IRequestHandler<GetFoodNutrientByFoodAndNutrientIdQuery, FoodNutrient?>
        {
            private readonly IFoodNutrientRepository _foodNutrientRepository;
            public GetFoodNutrientByFoodAndNutrientIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
            public async Task<FoodNutrient?> Handle(GetFoodNutrientByFoodAndNutrientIdQuery request, CancellationToken cancellationToken)
                => await _foodNutrientRepository.GetByFoodAndNutrientIdAsync(request.FoodId, request.NutrientId, cancellationToken);
        }
}