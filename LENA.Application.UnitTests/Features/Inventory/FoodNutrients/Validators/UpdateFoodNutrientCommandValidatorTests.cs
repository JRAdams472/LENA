using FluentValidation;
using LENA.Application.Features.Inventory.FoodNutrients.Commands;
using LENA.Application.Features.Inventory.FoodNutrients.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodNutrients.Validators
{
    public class UpdateFoodNutrientCommandValidatorTests
    {
        private readonly UpdateFoodNutrientCommandValidator _validator = new UpdateFoodNutrientCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateFoodNutrientCommand(new FoodNutrient());
            var result = _validator.Validate(command);
Assert.True(            result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_FoodNutrient_Is_Null()
        {
            var command = new UpdateFoodNutrientCommand(null!);
            var result = _validator.Validate(command);
Assert.False(            result.IsValid);
        }
    }
}
