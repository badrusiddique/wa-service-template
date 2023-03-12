using System.Net;
using FluentAssertions;
using Moq;
using Wati.Template.Api.Controllers;
using Wati.Template.Common.Dtos.Request;
using Wati.Template.Common.Dtos.Response;
using Wati.Template.Common.Enums;
using Wati.Template.Service.Services.Interfaces;

namespace Wati.Template.Api.UnitTests.Controllers
{
    public class DomainsControllerTests
    {
        private readonly DomainsController _testObject;
        private readonly Mock<IDomainOrchestrator> _mockOrchestrator;

        public DomainsControllerTests()
        {
            _mockOrchestrator = new Mock<IDomainOrchestrator>();

            _testObject = new DomainsController(_mockOrchestrator.Object);
        }

        #region GetAllAsync

        [Fact]
        public async void Verify_GetAllAsync_Returns_AllDomains()
        {
            // Arrange
            var stubData = new[]
            {
            new DomainResponseDto { Name = "sample-broker-1" },
            new DomainResponseDto { Name = "sample-broker-2" }
        };
            _mockOrchestrator.
                Setup(x => x.GetAllDomainsAsync())
                .ReturnsAsync(stubData)
                .Verifiable();

            // Act
            var result = await _testObject.GetAllAsync();

            // Assert
            _mockOrchestrator.Verify();
            var requestResult = result.Should().BeOfType<ApiResponse<IEnumerable<DomainResponseDto>>>().Which;
            requestResult.StatusCode.Should().Be(HttpStatusCode.OK);
            requestResult.Data.Should().BeEquivalentTo(stubData);
            requestResult.Error.Should().BeNull();
        }

        [Fact]
        public async void Verify_GetAllAsync_Returns_NotFound_If_DomainsIsNull()
        {
            // Arrange
            var errorStub = new ErrorResponse { Message = "the server could not find what was requested", Code = ErrorCode.NotFound.ToString() };
            _mockOrchestrator.
                Setup(x => x.GetAllDomainsAsync())
                .ReturnsAsync((IEnumerable<DomainResponseDto>)null)
                .Verifiable();

            // Act
            var result = await _testObject.GetAllAsync();

            // Assert
            _mockOrchestrator.Verify();
            var requestResult = result.Should().BeOfType<ApiResponse<IEnumerable<DomainResponseDto>>>().Which;
            requestResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
            requestResult.Data.Should().BeNull();
            requestResult.Error.Should().BeEquivalentTo(errorStub);
        }

        #endregion
    }
}
