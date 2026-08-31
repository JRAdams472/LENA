using FluentValidation;
using LENA.Application.Features.Inventory.FoodNutrients.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.FoodNutrients.Validators
{
    public class CreateFoodNutrientCommandValidator : AbstractValidator<CreateFoodNutrientCommand>
    {
        public CreateFoodNutrientCommandValidator()
        {
            RuleFor(x => x.FoodNutrient).NotNull().WithMessage("FoodNutrient is required");
        }
    }
}
