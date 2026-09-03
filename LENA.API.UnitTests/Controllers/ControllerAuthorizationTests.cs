using System.Reflection;

using LENA.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class ControllerAuthorizationTests
    {
        [Fact]
        public void AuthController_Me_Should_Be_Decorated_With_AuthorizeAttribute()
        {
            var method = typeof(AuthController).GetMethod("Me");

            Assert.NotNull(method);
            Assert.Single(method!.GetCustomAttributes(typeof(AuthorizeAttribute), false));
        }

        [Fact]
        public void Controllers_Should_Not_Have_AllowAnonymous_Actions()
        {
            var controllers = typeof(ControllerAuthorizationTests).Assembly
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .Append(typeof(AuthController).Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.IsSubclassOf(typeof(ControllerBase))
                            && t.Namespace == "LENA.API.Controllers")
                .ToList();

            var allowAnonymousActions = new List<string>();

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttributes(true).Any(a =>
                        a is HttpGetAttribute ||
                        a is HttpPostAttribute ||
                        a is HttpPutAttribute ||
                        a is HttpDeleteAttribute ||
                        a is HttpPatchAttribute ||
                        a is HttpHeadAttribute ||
                        a is HttpOptionsAttribute ||
                        a is AcceptVerbsAttribute ||
                        a is RouteAttribute));

                foreach (var method in methods)
                {
                    if (method.IsDefined(typeof(AllowAnonymousAttribute), false))
                    {
                        allowAnonymousActions.Add($"{controller.Name}.{method.Name}");
                    }
                }
            }

            Assert.Empty(allowAnonymousActions);
        }
    }
}