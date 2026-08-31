using FluentValidation;
using LENA.Application.Features.Inventory.FoodNutrients.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.FoodNutrients.Validators
{
    public class UpdateFoodNutrientCommandValidator : AbstractValidator<UpdateFoodNutrientCommand>
    {
        public UpdateFoodNutrientCommandValidator()
        {
            RuleFor(x => x.FoodNutrient).NotNull().WithMessage("FoodNutrient is required");
        }
    }
}
