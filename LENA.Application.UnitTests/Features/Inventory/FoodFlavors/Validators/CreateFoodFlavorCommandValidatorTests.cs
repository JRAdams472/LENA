using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Inventory.FoodFlavors.Commands;
using LENA.Application.Features.Inventory.FoodFlavors.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodFlavors.Validators
{
    public class CreateFoodFlavorCommandValidatorTests
    {
        private readonly CreateFoodFlavorCommandValidator _validator = new CreateFoodFlavorCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateFoodFlavorCommand(new FoodFlavor());
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_FoodFlavor_Is_Null()
        {
            var command = new CreateFoodFlavorCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}