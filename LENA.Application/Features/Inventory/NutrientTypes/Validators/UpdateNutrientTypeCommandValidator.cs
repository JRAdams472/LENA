using FluentValidation;

using LENA.Application.Features.Inventory.NutrientTypes.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.NutrientTypes.Validators
{
    public class UpdateNutrientTypeCommandValidator : AbstractValidator<UpdateNutrientTypeCommand>
    {
        public UpdateNutrientTypeCommandValidator()
        {
            RuleFor(x => x.NutrientType).NotNull().WithMessage("NutrientType is required");
        }
    }
}