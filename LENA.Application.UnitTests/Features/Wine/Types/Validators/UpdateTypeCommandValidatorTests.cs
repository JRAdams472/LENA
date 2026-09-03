using FluentValidation;

using LENA.Application.Features.Wine.Types.Commands;
using LENA.Application.Features.Wine.Types.Validators;
using LENA.Domain.Entity.Wine;

using Xunit;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.UnitTests.Features.Wine.Types.Validators
{
    public class UpdateTypeCommandValidatorTests
    {
        private readonly UpdateTypeCommandValidator _validator = new UpdateTypeCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateTypeCommand(new TypeEntity { TypeName = "Test" });
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_Type_Is_Null()
        {
            var command = new UpdateTypeCommand(null!);
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
        }
    }
}