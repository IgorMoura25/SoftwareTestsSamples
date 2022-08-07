using System.Diagnostics.CodeAnalysis;
using Xunit;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Extensions;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Exceptions;

namespace IDSIM.Services.UnitTest.SharedKernelTests
{
    [ExcludeFromCodeCoverage]
    public class RegionExtensionsTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public RegionExtensionsTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void CanOperate_RegionThatCanOperate_ShouldReturnTrue()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();

            // Act
            var response = validRegion.CanOperate();

            // Assert
            Assert.True(response);
        }

        [Fact]
        public void CanOperate_RegionThatCannotOperate_ShouldThrowException()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();

            // Act & Assert
            Assert.Throws<RegionOperationNotAllowedException>(() => invalidRegion.CanOperate());
        }

        [Fact]
        public void CanOperate_FalseArgumentForRegionThatCannotOperate_ShouldReturnFalse()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();

            // Act
            var response = invalidRegion.CanOperate(false);

            // Assert
            Assert.False(response);
        }
    }
}
