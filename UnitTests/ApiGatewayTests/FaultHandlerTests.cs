using System.Diagnostics.CodeAnalysis;
using Xunit;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;
using Moq;

namespace IDSIM.Services.UnitTest.ApiGatewayTests
{

    [ExcludeFromCodeCoverage]
    public class FaultHandlerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public FaultHandlerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void HandleError_ValidDto_ShouldReturnFullMessageError()
        {
            // Arrange
            var faultDto = _unitApiTestsFixture.GetValidWebServiceCallFaultDto();

            // Act
            var response = FaultHandler.HandleError<It.IsAnyType>(faultDto);

            // Assert
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("Fault code: Test Faultcode | Message: Test Faultstring | DetailCode: Test DetailCode | DetailLevel: Test DetailLevel | DetailMessage: Test DetailMessage", response.Errors[0]);
        }

        [Fact]
        public void HandleError_NullDto_ShouldReturnEmptyMessageError()
        {
            // Arrange & Act
            var response = FaultHandler.HandleError<It.IsAnyType>(new Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.DTO.WebServiceCallFaultDto());

            // Assert
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("Fault code:  | Message:  | DetailCode:  | DetailLevel:  | DetailMessage: ", response.Errors[0]);
        }

        [Fact]
        public void HandleError_AnyText_ShouldReturnSameTextAsError()
        {
            // Arrange & Act
            var response = FaultHandler.HandleError<It.IsAnyType>("Test error");

            // Assert
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("Test error", response.Errors[0]);
        }
    }
}
