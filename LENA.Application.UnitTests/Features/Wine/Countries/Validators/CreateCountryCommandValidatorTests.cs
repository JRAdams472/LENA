using FluentValidation;

using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Application.Features.Wine.Countries.Validators;
using LENA.Domain.Entity.Wine;

using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Countries.Validators
{
    public class CreateCountryCommandValidatorTests
    {
        private readonly CreateCountryCommandValidator _validator = new CreateCountryCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateCountryCommand(new Country { CountryName = "Test", ISOCode = "XX" });
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_Country_Is_Null()
        {
            var command = new CreateCountryCommand(null!);
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
        }
    }
}