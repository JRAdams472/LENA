using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Inventory.FoodNutrients.Commands;
using LENA.Application.Features.Inventory.FoodNutrients.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodNutrients.Validators
{
    public class CreateFoodNutrientCommandValidatorTests
    {
        private readonly CreateFoodNutrientCommandValidator _validator = new CreateFoodNutrientCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateFoodNutrientCommand(new FoodNutrient());
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_FoodNutrient_Is_Null()
        {
            var command = new CreateFoodNutrientCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}