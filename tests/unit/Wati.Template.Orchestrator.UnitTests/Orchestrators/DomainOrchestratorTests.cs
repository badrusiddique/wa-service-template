using AutoMapper;
using FluentAssertions;
using Moq;
using Wati.Template.Common.Dtos.Request;
using Wati.Template.Service.Services.Interfaces;

namespace Wati.Template.Orchestrator.UnitTests.Orchestrators
{
    public class DomainOrchestratorTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly IDomainOrchestrator _testObject;
        private readonly Mock<IDomainService> _mockDomainService;

        public DomainOrchestratorTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockDomainService = new Mock<IDomainService>();

            _testObject = new DomainOrchestrator(_mockMapper.Object, _mockDomainService.Object);
        }

        #region GetAllDomainsAsync

        [Fact]
        public async void Verify_GetAllDomainsAsync_Returns_AllDomains()
        {
            // Arrange
            var models = new[]
            {
                new DomainModel { Name = "sample-broker-1" },
                new DomainModel { Name = "sample-broker-2" }
            };
            var dtos = new[]
            {
                new DomainResponseDto { Name = "sample-broker-1" },
                new DomainResponseDto { Name = "sample-broker-2" }
            };
            _mockDomainService.
                Setup(x => x.GetAllDomainsAsync())
                .ReturnsAsync(models)
                .Verifiable();
            _mockMapper
                .Setup(x => x.Map<IEnumerable<DomainResponseDto>>(models))
                .Returns(dtos)
                .Verifiable();

            // Act
            var result = await _testObject.GetAllDomainsAsync();

            // Assert
            _mockMapper.Verify();
            _mockDomainService.Verify();
            result.Should().BeEquivalentTo(dtos);
        }

        #endregion
    }
}
