using FluentValidation;
using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Regions.Validators
{
    public class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator()
        {
            RuleFor(x => x.Region).NotNull().WithMessage("Region is required");
        }
    }
}
