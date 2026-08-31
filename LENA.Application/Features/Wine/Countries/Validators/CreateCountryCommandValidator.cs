using FluentValidation;
using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Countries.Validators
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(x => x.Country).NotNull().WithMessage("Country is required");
        }
    }
}
