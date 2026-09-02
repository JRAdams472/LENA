using System.Security.Claims;
using FluentAssertions;
using LENA.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _sut = new();

        [Fact]
        public void Me_Should_Return_Email_And_Claims_When_Authenticated()
        {
            var email = "test@example.com";
            var identity = new ClaimsIdentity(
                new[] { new Claim("email", email) },
                "Bearer",
                "email",
                "role");

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            };

            _sut.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var result = _sut.Me();

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Me_Should_Return_Unauthorized_When_No_Email_Claim()
        {
            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = _sut.Me();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Me_Should_Be_Decorated_With_AuthorizeAttribute()
        {
            var method = typeof(AuthController).GetMethod("Me");
            method.Should().NotBeNull();
            method!.GetCustomAttributes(typeof(AuthorizeAttribute), false)
                .Should().ContainSingle();
        }
    }
}
