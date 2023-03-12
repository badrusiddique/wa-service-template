using System.Net;
using FluentAssertions;
using Wati.Template.Api;
using Wati.Template.Common.Dtos.Request;
using Wati.Template.Common.Dtos.Response;

namespace Wati.Template.Service.IntegrationTests.Endpoints
{
    public class DomainsEndpointTests : BaseEndpointTests
    {
        public DomainsEndpointTests(CustomWebAppFactory<Program> factory) : base(factory)
        {
        }

        #region Integration test

        [Fact]
        public async Task Verify_GetAllDomainsAsync()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            var response = await client.GetAsync(Constant.DomainsUrl);
            var responseDto = await ParseResponse<ApiResponse<IEnumerable<DomainResponseDto>>>(response);

            // Assert
            responseDto.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("Domains/All_Domains.json")]
        public async Task Verify_GetAllDomainsAsync_WithData(string fileName)
        {
            // Arrange
            using var client = CreateClient();
            var expected = ParseEndpointData<ApiResponse<IEnumerable<DomainResponseDto>>>(fileName);

            // Act
            var response = await client.GetAsync(Constant.DomainsUrl);
            var responseDto = await ParseResponse<ApiResponse<IEnumerable<DomainResponseDto>>>(response);

            // Assert
            responseDto.Should().NotBeNull();
            responseDto
                .Should()
                .BeEquivalentTo(
                    expected, o => o
                        .Excluding(x => x.SelectedMemberPath.EndsWith("LastUpdatedAt")));
        }

        #endregion
    }
}
