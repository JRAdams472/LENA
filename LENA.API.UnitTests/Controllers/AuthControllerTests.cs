using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Controllers;
using LENA.Application.Contracts.Auditing;
using LENA.Application.Features.Identity.Users.Queries;
using LENA.Domain.Entity.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private static AuthController CreateSut(User? currentUser = null)
        {
            var mediator = new Mock<MediatR.IMediator>();
            mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(currentUser);

            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.SetupGet(u => u.UserName).Returns(currentUser?.Email ?? string.Empty);

            return new AuthController(mediator.Object, currentUserService.Object);
        }

        [Fact]
        public async Task Me_Should_Return_User_When_Authenticated()
        {
            var email = "test@example.com";
            var user = new User
            {
                UserID = 1,
                ExternalSubject = "sub123",
                Provider = "google",
                Email = email,
                DisplayName = "Test User",
            };

            var sut = CreateSut(user);
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("email", email) }, "Bearer")),
                },
            };

            var result = await sut.Me(CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Me_Should_Return_Unauthorized_When_No_Current_User()
        {
            var sut = CreateSut(currentUser: null);
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
            };

            var result = await sut.Me(CancellationToken.None);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void Me_Should_Be_Decorated_With_AuthorizeAttribute()
        {
            var method = typeof(AuthController).GetMethod("Me");
            Assert.NotNull(method);
            Assert.Single(method!.GetCustomAttributes(typeof(AuthorizeAttribute), false));
        }
    }
}
