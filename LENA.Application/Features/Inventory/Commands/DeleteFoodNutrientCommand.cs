using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Commands
{
    public record DeleteFoodNutrientCommand(int FoodNutrientId) : IRequest<FoodNutrient?>;

        public class DeleteFoodNutrientCommandHandler : IRequestHandler<DeleteFoodNutrientCommand, FoodNutrient?>
        {
            private readonly IFoodNutrientRepository _foodNutrientRepository;
            public DeleteFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
            public async Task<FoodNutrient?> Handle(DeleteFoodNutrientCommand request, CancellationToken cancellationToken)
            {
                var foodNutrient = await _foodNutrientRepository.GetByIdAsync(request.FoodNutrientId);
                if (foodNutrient == null)
                    return null;
    
                return await _foodNutrientRepository.DeleteAsync(foodNutrient);
            }
        }
}