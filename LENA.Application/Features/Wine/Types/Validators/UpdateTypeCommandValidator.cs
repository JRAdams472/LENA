using FluentValidation;

using LENA.Application.Features.Wine.Types.Commands;
using LENA.Domain.Entity.Wine;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Validators
{
    public class UpdateTypeCommandValidator : AbstractValidator<UpdateTypeCommand>
    {
        public UpdateTypeCommandValidator()
        {
            RuleFor(x => x.Type).NotNull().WithMessage("Type is required");
        }
    }
}