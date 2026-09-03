using FluentValidation;

using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Regions.Validators
{
    public class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidator()
        {
            RuleFor(x => x.Region).NotNull().WithMessage("Region is required");
        }
    }
}