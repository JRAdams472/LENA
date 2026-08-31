using FluentValidation;
using LENA.Application.Features.Inventory.FoodFlavors.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.FoodFlavors.Validators
{
    public class UpdateFoodFlavorCommandValidator : AbstractValidator<UpdateFoodFlavorCommand>
    {
        public UpdateFoodFlavorCommandValidator()
        {
            RuleFor(x => x.FoodFlavor).NotNull().WithMessage("FoodFlavor is required");
        }
    }
}