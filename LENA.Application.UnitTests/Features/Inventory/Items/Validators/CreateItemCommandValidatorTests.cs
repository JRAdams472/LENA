using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Application.Features.Inventory.Items.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.Items.Validators
{
    public class CreateItemCommandValidatorTests
    {
        private readonly CreateItemCommandValidator _validator = new CreateItemCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateItemCommand(new Item { Name = "Test", Unit = "ea" });
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_Item_Is_Null()
        {
            var command = new CreateItemCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}