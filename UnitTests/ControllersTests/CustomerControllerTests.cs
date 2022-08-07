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
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Customer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Customer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class CustomerControllerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public CustomerControllerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task SearchCustomerAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidSearchCustomerRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new CustomerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchCustomerAsync(It.IsAny<SearchCustomerIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchCustomerResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchCustomerAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchCustomerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchCustomerAsync(It.IsAny<SearchCustomerIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task SearchCustomerAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new CustomerController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.SearchCustomerAsync(It.IsAny<SearchCustomerIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidSearchCustomerResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchCustomerAsync(new SearchCustomerIMRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchCustomerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchCustomerAsync(It.IsAny<SearchCustomerIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchCustomerAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new CustomerController(dependency.Object)
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
            var response = await controller.SearchCustomerAsync(new SearchCustomerIMRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchCustomerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchCustomerAsync(It.IsAny<SearchCustomerIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateCustomerAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidCreateCustomerRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new CustomerController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.CreateCustomerAsync(It.IsAny<CreateCustomerIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateCustomerResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateCustomerAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateCustomerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateCustomerAsync(It.IsAny<CreateCustomerIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task CreateCustomerAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new CustomerController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.CreateCustomerAsync(It.IsAny<CreateCustomerIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidCreateCustomerResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateCustomerAsync(new CreateCustomerRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateCustomerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateCustomerAsync(It.IsAny<CreateCustomerIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateCustomerAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new CustomerController(dependency.Object)
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
            var response = await controller.CreateCustomerAsync(new CreateCustomerRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateCustomerResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateCustomerAsync(It.IsAny<CreateCustomerIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

    }
}
