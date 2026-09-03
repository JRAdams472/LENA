using LENA.Application.Contracts.Auditing;
using LENA.Application.Features.Identity.Users.Commands;
using LENA.Domain.Entity.Identity;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.API.Middleware
{
    public class UserResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public UserResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMediator mediator, ICurrentUserService currentUser, IMemoryCache cache)
        {
            var externalSubject = currentUser.ExternalSubject;
            var email = currentUser.UserName;

            if (!string.IsNullOrWhiteSpace(externalSubject) && !string.IsNullOrWhiteSpace(email))
            {
                var cacheKey = $"user:{externalSubject}";
                var user = await cache.GetOrCreateAsync(
                    cacheKey,
                    async _ =>
                    {
                        var displayName = context.User.FindFirst("name")?.Value;
                        return await mediator.Send(
                            new UpsertUserCommand(externalSubject, "google", email, displayName),
                            context.RequestAborted);
                    },
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));

                if (user is not null)
                {
                    context.Items["UserID"] = user.UserID;
                }
            }

            await _next(context);
        }
    }
}