using FluentValidation;

using LENA.Application.Features.Inventory.NutrientTypes.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.NutrientTypes.Validators
{
    public class CreateNutrientTypeCommandValidator : AbstractValidator<CreateNutrientTypeCommand>
    {
        public CreateNutrientTypeCommandValidator()
        {
            RuleFor(x => x.NutrientType).NotNull().WithMessage("NutrientType is required");
        }
    }
}