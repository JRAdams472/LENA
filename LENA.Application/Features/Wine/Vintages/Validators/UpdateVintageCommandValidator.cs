using FluentValidation;
using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Vintages.Validators
{
    public class UpdateVintageCommandValidator : AbstractValidator<UpdateVintageCommand>
    {
        public UpdateVintageCommandValidator()
        {
            RuleFor(x => x.Vintage).NotNull().WithMessage("Vintage is required");
        }
    }
}
