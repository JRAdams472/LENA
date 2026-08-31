using FluentValidation;
using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Bottles.Validators
{
    public class CreateBottleCommandValidator : AbstractValidator<CreateBottleCommand>
    {
        public CreateBottleCommandValidator()
        {
            RuleFor(x => x.Bottle).NotNull().WithMessage("Bottle is required");
        }
    }
}