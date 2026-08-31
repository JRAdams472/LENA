using FluentValidation;
using LENA.Application.Features.Inventory.FoodFlavors.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.FoodFlavors.Validators
{
    public class CreateFoodFlavorCommandValidator : AbstractValidator<CreateFoodFlavorCommand>
    {
        public CreateFoodFlavorCommandValidator()
        {
            RuleFor(x => x.FoodFlavor).NotNull().WithMessage("FoodFlavor is required");
        }
    }
}