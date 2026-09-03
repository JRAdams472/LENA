using LENA.Application.Contracts.Auditing;
using LENA.Application.Features.Identity.Users.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public AuthController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me(CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetCurrentUserQuery(), cancellationToken);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                user.UserID,
                user.Email,
                user.DisplayName,
                user.ExternalSubject,
                user.Provider,
            });
        }
    }
}