using FluentValidation;
using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Features.Wine.Countries.Validators
{
    public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(x => x.Country).NotNull().WithMessage("Country is required");
        }
    }
}