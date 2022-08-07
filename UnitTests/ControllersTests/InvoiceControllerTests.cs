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
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Invoice;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Invoice;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class InvoiceControllerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public InvoiceControllerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task SearchByContractAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validSearchByContractRequestModel = _unitApiTestsFixture.GetValidInvoiceSearchByContractRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new InvoiceController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchInvoicesAsync(It.IsAny<SearchInvoicesRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchInvoicesResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchByContractAsync(validSearchByContractRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchInvoicesResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchInvoicesAsync(It.IsAny<SearchInvoicesRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task SearchByContractAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new InvoiceController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.SearchInvoicesAsync(It.IsAny<SearchInvoicesRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidSearchInvoicesResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchByContractAsync(new InvoiceSearchByContractRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchInvoicesResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchInvoicesAsync(It.IsAny<SearchInvoicesRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchByContractAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new InvoiceController(dependency.Object)
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
            var response = await controller.SearchByContractAsync(new InvoiceSearchByContractRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchInvoicesResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchInvoicesAsync(It.IsAny<SearchInvoicesRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task UpdatePromiseToPayAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidUpdatePromiseToPayRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new InvoiceController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidUpdatePromiseToPayDateResponseModelFacadeResponse()));

            // Act
            var response = await controller.UpdatePromiseToPayAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<UpdatePromiseToPayDateResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task UpdatePromiseToPayAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new InvoiceController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidUpdatePromiseToPayDateResponseModelFacadeResponse()));

            // Act
            var response = await controller.UpdatePromiseToPayAsync(new UpdatePromiseToPayRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<UpdatePromiseToPayDateResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task UpdatePromiseToPayAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new InvoiceController(dependency.Object)
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
            var response = await controller.UpdatePromiseToPayAsync(new UpdatePromiseToPayRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<UpdatePromiseToPayDateResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }
    }
}
