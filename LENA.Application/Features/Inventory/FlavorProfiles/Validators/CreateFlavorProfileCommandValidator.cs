using FluentValidation;

using LENA.Application.Features.Inventory.FlavorProfiles.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Validators
{
    public class CreateFlavorProfileCommandValidator : AbstractValidator<CreateFlavorProfileCommand>
    {
        public CreateFlavorProfileCommandValidator()
        {
            RuleFor(x => x.FlavorProfile).NotNull().WithMessage("FlavorProfile is required");
        }
    }
}