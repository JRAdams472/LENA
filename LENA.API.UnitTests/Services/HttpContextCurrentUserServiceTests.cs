using LENA.API.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace LENA.API.UnitTests.Services
{
    public class HttpContextCurrentUserServiceTests
    {
        [Fact]
        public void UserID_Throws_When_Not_Resolved()
        {
            var contextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
            var service = new HttpContextCurrentUserService(contextAccessor);

            Assert.Throws<LENA.Application.Exceptions.UnauthenticatedUserException>(() => service.UserID);
        }

        [Fact]
        public void UserID_Throws_When_Resolved_To_Zero()
        {
            var contextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
            contextAccessor.HttpContext.Items["UserID"] = 0;
            var service = new HttpContextCurrentUserService(contextAccessor);

            Assert.Throws<LENA.Application.Exceptions.UnauthenticatedUserException>(() => service.UserID);
        }

        [Fact]
        public void UserID_Returns_Value_When_Resolved()
        {
            var contextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
            contextAccessor.HttpContext.Items["UserID"] = 42;
            var service = new HttpContextCurrentUserService(contextAccessor);

            Assert.Equal(42, service.UserID);
        }
    }
}
