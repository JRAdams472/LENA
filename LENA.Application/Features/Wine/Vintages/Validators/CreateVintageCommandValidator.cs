using FluentValidation;
using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Vintages.Validators
{
    public class CreateVintageCommandValidator : AbstractValidator<CreateVintageCommand>
    {
        public CreateVintageCommandValidator()
        {
            RuleFor(x => x.Vintage).NotNull().WithMessage("Vintage is required");
        }
    }
}
