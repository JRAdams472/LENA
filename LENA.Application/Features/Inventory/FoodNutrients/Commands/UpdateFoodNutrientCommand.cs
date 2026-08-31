using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Commands
{
    public record UpdateFoodNutrientCommand(FoodNutrient FoodNutrient) : IRequest<FoodNutrient>;

    public class UpdateFoodNutrientCommandHandler : IRequestHandler<UpdateFoodNutrientCommand, FoodNutrient>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public UpdateFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient> Handle(UpdateFoodNutrientCommand request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.UpdateAsync(request.FoodNutrient, cancellationToken);
    }
}
