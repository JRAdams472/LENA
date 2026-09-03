using System.Security.Claims;

using LENA.Application.Contracts.Auditing;

namespace LENA.API.Services
{
    public class HttpContextCurrentUserService : ICurrentUserService
    {
        private const string AnonymousUserName = "system";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserName
        {
            get
            {
                var name = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                return string.IsNullOrWhiteSpace(name) ? AnonymousUserName : name;
            }
        }

        public int UserID
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context?.Items.TryGetValue("UserID", out var value) == true && value is int userId && userId > 0)
                {
                    return userId;
                }

                throw new LENA.Application.Exceptions.UnauthenticatedUserException();
            }
        }

        public string? ExternalSubject
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null)
                {
                    return null;
                }

                return user.FindFirst("sub")?.Value
                    ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }
    }
}