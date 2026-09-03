using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using LENA.Application.Exceptions;

namespace LENA.Application.Features.Inventory.FoodNutrients.Commands
{
    public record DeleteFoodNutrientCommand(int FoodId, int NutrientId) : IRequest<FoodNutrient?>;

    public class DeleteFoodNutrientCommandHandler : IRequestHandler<DeleteFoodNutrientCommand, FoodNutrient?>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public DeleteFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient?> Handle(DeleteFoodNutrientCommand request, CancellationToken cancellationToken)
        {
            var foodNutrient = await _foodNutrientRepository.GetByFoodAndNutrientIdAsync(request.FoodId, request.NutrientId, cancellationToken) ?? throw new NotFoundException(nameof(FoodNutrient), ($"{request.FoodId}-{request.NutrientId}"));

            return await _foodNutrientRepository.DeleteAsync(foodNutrient, cancellationToken);
        }
    }
}
