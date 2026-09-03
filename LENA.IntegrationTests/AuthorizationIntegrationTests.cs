using System.Net;
using System.Net.Http.Headers;

using LENA.IntegrationTests.Infrastructure;

using Xunit;

namespace LENA.IntegrationTests
{
    public class AuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthorizationIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ProtectedDataEndpoint_WithoutAuthorization_Returns401()
        {
            var response = await _client.GetAsync("/api/Item/items");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AuthMe_WithoutAuthorization_Returns401()
        {
            var response = await _client.GetAsync("/api/auth/me");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AuthMe_WithValidTestToken_ReturnsCurrentUser()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TestAuthHandler.TestToken);

            var response = await _client.GetAsync("/api/auth/me");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("test@example.com", content);
            Assert.Contains("Test User", content);
        }
    }
}