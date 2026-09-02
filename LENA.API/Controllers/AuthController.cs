using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new { Email = email, Claims = claims });
        }
    }
}
