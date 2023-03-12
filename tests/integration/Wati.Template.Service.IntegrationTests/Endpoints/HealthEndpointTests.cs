using Wati.Template.Api;

namespace Wati.Template.Service.IntegrationTests.Endpoints
{
    public class HealthEndpointTests : BaseEndpointTests
    {
        public HealthEndpointTests(CustomWebAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Verify_Health_Ping()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            var response = await client.GetAsync(Constant.HealthPingUrl);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Verify_Health_Checks()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            var response = await client.GetAsync(Constant.HealthChecksUrl);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
