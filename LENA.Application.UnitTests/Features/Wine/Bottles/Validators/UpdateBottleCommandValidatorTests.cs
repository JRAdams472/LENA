using FluentValidation;

using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Application.Features.Wine.Bottles.Validators;
using LENA.Domain.Entity.Wine;

using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Bottles.Validators
{
    public class UpdateBottleCommandValidatorTests
    {
        private readonly UpdateBottleCommandValidator _validator = new UpdateBottleCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateBottleCommand(new Bottle());
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_Bottle_Is_Null()
        {
            var command = new UpdateBottleCommand(null!);
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
        }
    }
}