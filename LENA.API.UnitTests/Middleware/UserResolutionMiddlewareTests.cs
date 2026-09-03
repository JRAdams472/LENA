using System.Threading;
using System.Threading.Tasks;

using LENA.API.Middleware;
using LENA.Application.Contracts.Auditing;
using LENA.Application.Features.Identity.Users.Commands;
using LENA.Domain.Entity.Identity;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using Xunit;

namespace LENA.API.UnitTests.Middleware
{
    public class UserResolutionMiddlewareTests
    {
        [Fact]
        public async Task Same_Subject_Does_Not_Send_Second_Upsert()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<UpsertUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { UserID = 42, ExternalSubject = "sub-123", Provider = "google", Email = "test@example.com" });

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(u => u.ExternalSubject).Returns("sub-123");
            currentUser.SetupGet(u => u.UserName).Returns("test@example.com");

            var cache = new MemoryCache(new MemoryCacheOptions());
            var context = new DefaultHttpContext();

            var middleware = new UserResolutionMiddleware(_ => Task.CompletedTask);

            await middleware.InvokeAsync(context, mediator.Object, currentUser.Object, cache);
            await middleware.InvokeAsync(context, mediator.Object, currentUser.Object, cache);

            mediator.Verify(m => m.Send(It.IsAny<UpsertUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}