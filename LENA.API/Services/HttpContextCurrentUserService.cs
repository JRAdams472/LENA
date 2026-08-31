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
    }
}
