using FluentValidation;
using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Bottles.Validators
{
    public class UpdateBottleCommandValidator : AbstractValidator<UpdateBottleCommand>
    {
        public UpdateBottleCommandValidator()
        {
            RuleFor(x => x.Bottle).NotNull().WithMessage("Bottle is required");
        }
    }
}
