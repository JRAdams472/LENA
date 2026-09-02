using FluentValidation;
using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Application.Features.Inventory.Items.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.Items.Validators
{
    public class UpdateItemCommandValidatorTests
    {
        private readonly UpdateItemCommandValidator _validator = new UpdateItemCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateItemCommand(new Item { Name = "Test", Unit = "ea" });
            var result = _validator.Validate(command);
Assert.True(            result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_Item_Is_Null()
        {
            var command = new UpdateItemCommand(null!);
            var result = _validator.Validate(command);
Assert.False(            result.IsValid);
        }
    }
}
