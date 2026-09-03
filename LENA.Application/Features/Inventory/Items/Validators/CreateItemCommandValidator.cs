using FluentValidation;

using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Features.Inventory.Items.Validators
{
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.Item).NotNull().WithMessage("Item is required");
        }
    }
}