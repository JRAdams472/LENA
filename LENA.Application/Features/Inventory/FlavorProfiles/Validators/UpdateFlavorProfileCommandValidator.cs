using FluentValidation;
using LENA.Application.Features.Inventory.FlavorProfiles.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Validators
{
    public class UpdateFlavorProfileCommandValidator : AbstractValidator<UpdateFlavorProfileCommand>
    {
        public UpdateFlavorProfileCommandValidator()
        {
            RuleFor(x => x.FlavorProfile).NotNull().WithMessage("FlavorProfile is required");
        }
    }
}
