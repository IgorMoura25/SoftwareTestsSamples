using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;

namespace IDSIM.Services.UnitTest.ApiGatewayTests.Models
{
    [ExcludeFromCodeCoverage]
    public class SearchContractsRequestModelTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public SearchContractsRequestModelTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void Validate_ValidModel_ShouldNotReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetValidSearchContractsRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(0, facadeResponse.Errors?.Count);
        }

        [Fact]
        public void Validate_InvalidModel_ShouldReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetInvalidSearchContractsRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(5, facadeResponse.Errors?.Count);
        }
    }
}
