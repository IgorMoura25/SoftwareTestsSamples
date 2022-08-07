using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Dell.DFS.LLS.IntegrationManagerServices.Api;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers.V1;

namespace IDSIM.Services.UnitTest.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class AdminControllerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public AdminControllerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void GetRegionStatus_ValidRegion_ShouldReturnOk()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var controller = new AdminController();

            // Act
            var response = controller.GetRegionStatus(validRegion.ToString());

            // Assert
            Assert.True(validRegion.IsActive);
            Assert.NotNull(response);
            Assert.Equal("OK", response.ResponseCode);
            Assert.Null(response.CorrelationID);
            Assert.Null(response.Errors);
            Assert.NotEqual(System.DateTime.MinValue, response.Timestamp);
            Assert.Equal(typeof(Startup).Namespace, response.Origin);
        }

        [Fact]
        public void GetRegionStatus_InvalidRegion_ShouldReturnBadRequest()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var controller = new AdminController();

            // Act
            var response = controller.GetRegionStatus(invalidRegion.ToString());

            // Assert
            Assert.False(invalidRegion.IsActive);
            Assert.NotNull(response);
            Assert.Equal("BadRequest", response.ResponseCode);
        }

        [Fact]
        public void GetRegions_ShouldReturnOk()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var controller = new AdminController();

            // Act
            var response = controller.GetRegions();

            // Assert
            var regions = (List<RegionStatusModel>?)response?.Response;
            var regionFound = regions.Find(x => x.Name == validRegion.Name);

            Assert.NotNull(response);
            Assert.NotNull(regions);
            Assert.NotNull(regionFound);
            Assert.Equal(validRegion.Name, regionFound.Name);
            Assert.Equal("OK", response.ResponseCode);
            Assert.Null(response.CorrelationID);
            Assert.Null(response.Errors);
            Assert.NotEqual(System.DateTime.MinValue, response.Timestamp);
            Assert.Equal(typeof(Startup).Namespace, response.Origin);
        }
    }
}
