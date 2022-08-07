using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;

namespace IDSIM.Services.UnitTest.ApiGatewayTests.Models
{
    [ExcludeFromCodeCoverage]
    public class SearchMessagesRequestModelTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public SearchMessagesRequestModelTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void Validate_ValidModel_ShouldNotReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetValidSearchMessagesIMRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(0, facadeResponse.Errors?.Count);
        }

        [Fact]
        public void Validate_InvalidModel_ShouldReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetInvalidSearchMessagesIMRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(2, facadeResponse.Errors?.Count);
        }
    }
}
