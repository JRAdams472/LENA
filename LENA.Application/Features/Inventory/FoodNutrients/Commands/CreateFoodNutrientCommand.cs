using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Commands
{
    public record CreateFoodNutrientCommand(FoodNutrient FoodNutrient) : IRequest<FoodNutrient>;

    public class CreateFoodNutrientCommandHandler : IRequestHandler<CreateFoodNutrientCommand, FoodNutrient>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public CreateFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient> Handle(CreateFoodNutrientCommand request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.CreateAsync(request.FoodNutrient, cancellationToken);
    }
}