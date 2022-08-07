using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace IDSIM.Services.UnitTest.ApiGatewayTests.Models
{
    [ExcludeFromCodeCoverage]
    public class GetLinkGetContractLinkCodeDetailsByCCANRequestModelTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public GetLinkGetContractLinkCodeDetailsByCCANRequestModelTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void Validate_ValidModel_ShouldNotReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(0, facadeResponse.Errors?.Count);
        }

        [Fact]
        public void Validate_InvalidModel_ShouldReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetInvalidGetContractLinkCodeDetailsByCCANRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(2, facadeResponse.Errors?.Count);
        }
    }
}
