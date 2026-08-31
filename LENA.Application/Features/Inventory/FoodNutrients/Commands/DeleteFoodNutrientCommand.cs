using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Commands
{
    public record DeleteFoodNutrientCommand(int FoodId, int NutrientId) : IRequest<FoodNutrient?>;

    public class DeleteFoodNutrientCommandHandler : IRequestHandler<DeleteFoodNutrientCommand, FoodNutrient?>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public DeleteFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient?> Handle(DeleteFoodNutrientCommand request, CancellationToken cancellationToken)
        {
            var foodNutrient = await _foodNutrientRepository.GetByFoodAndNutrientIdAsync(request.FoodId, request.NutrientId, cancellationToken);
            if (foodNutrient == null)
                return null;

            return await _foodNutrientRepository.DeleteAsync(foodNutrient, cancellationToken);
        }
    }
}