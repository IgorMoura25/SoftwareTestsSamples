using System.Net;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers.V1;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Dealer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Dealer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class DealerControllerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public DealerControllerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task SearchAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidSearchDealerViewRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();
            
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new DealerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchDealerResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchDealerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task SearchAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new DealerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade)
                .Returns(integrationManager.Object);

            dependency
                .Setup(c => c.Log)
                .Returns(log.Object);

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidSearchDealerResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchAsync(new SearchDealerViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchDealerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new DealerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade)
                .Returns(integrationManager.Object);

            dependency
                .Setup(c => c.Log)
                .Returns(log.Object);

            // Act
            var response = await controller.SearchAsync(new SearchDealerViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchDealerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidCreateDealerViewRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();
            
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new DealerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateDealerResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateDealerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new DealerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade)
                .Returns(integrationManager.Object);

            dependency
                .Setup(c => c.Log)
                .Returns(log.Object);

            dependency
                .Setup(c => c.IntegrationManagerFacade.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidCreateDealerResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateAsync(new CreateDealerViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateDealerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new DealerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade)
                .Returns(integrationManager.Object);

            dependency
                .Setup(c => c.Log)
                .Returns(log.Object);

            // Act
            var response = await controller.CreateAsync(new CreateDealerViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateDealerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }
    }
}
