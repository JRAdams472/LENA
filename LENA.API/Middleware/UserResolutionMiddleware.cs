using LENA.Application.Contracts.Auditing;
using LENA.Application.Features.Identity.Users.Commands;
using MediatR;

namespace LENA.API.Middleware
{
    public class UserResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public UserResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMediator mediator, ICurrentUserService currentUser)
        {
            var externalSubject = currentUser.ExternalSubject;
            var email = currentUser.UserName;

            if (!string.IsNullOrWhiteSpace(externalSubject) && !string.IsNullOrWhiteSpace(email))
            {
                var displayName = context.User.FindFirst("name")?.Value;

                var user = await mediator.Send(
                    new UpsertUserCommand(externalSubject, "google", email, displayName),
                    context.RequestAborted);

                context.Items["UserID"] = user.UserID;
            }

            await _next(context);
        }
    }
}
