using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace IDSIM.Services.UnitTest.ApiGatewayTests.Models
{
    [ExcludeFromCodeCoverage]
    public class CreateParentRequestModelTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public CreateParentRequestModelTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void Validate_ValidModel_ShouldNotReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetValidCreateParentRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(0, facadeResponse.Errors?.Count);
        }

        [Fact]
        public void Validate_InvalidModel_ShouldReturnErrors()
        {
            // Arrange
            var validModel = _unitApiTestsFixture.GetInvalidCreateParentRequestModel();

            // Act
            var facadeResponse = validModel.Validate<It.IsAnyType>();

            // Assert
            Assert.Equal(1, facadeResponse.Errors?.Count);
        }
    }
}
