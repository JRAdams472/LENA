using FluentValidation;
using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Application.Features.Wine.Vintages.Validators;
using LENA.Domain.Entity.Wine;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Vintages.Validators
{
    public class UpdateVintageCommandValidatorTests
    {
        private readonly UpdateVintageCommandValidator _validator = new UpdateVintageCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateVintageCommand(new Vintage());
            var result = _validator.Validate(command);
Assert.True(            result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_Vintage_Is_Null()
        {
            var command = new UpdateVintageCommand(null!);
            var result = _validator.Validate(command);
Assert.False(            result.IsValid);
        }
    }
}
