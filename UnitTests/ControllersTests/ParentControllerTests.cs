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
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Parent;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Parent;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class ParentControllerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public ParentControllerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task ListAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ParentController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.ListParentsAsync(It.IsAny<ListParentsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidListParentsResponseModelFacadeResponse()));

            // Act
            var response = await controller.ListAsync();

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<ListParentsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.ListParentsAsync(It.IsAny<ListParentsRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task ListAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ParentController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.ListParentsAsync(It.IsAny<ListParentsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidListParentsResponseModelFacadeResponse()));

            // Act
            var response = await controller.ListAsync();

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<ListParentsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.ListParentsAsync(It.IsAny<ListParentsRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task ListAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ParentController(dependency.Object)
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
            var response = await controller.ListAsync();

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<ListParentsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.ListParentsAsync(It.IsAny<ListParentsRequestModel>()), Times.Once);
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
            var validRequestModel = _unitApiTestsFixture.GetValidCreateParentViewRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ParentController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.CreateParentAsync(It.IsAny<CreateParentRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateParentResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateParentResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateParentAsync(It.IsAny<CreateParentRequestModel>()), Times.Once);
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

            var controller = new ParentController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.CreateParentAsync(It.IsAny<CreateParentRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidCreateParentResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateAsync(new CreateParentViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateParentResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateParentAsync(It.IsAny<CreateParentRequestModel>()), Times.Once);
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

            var controller = new ParentController(dependency.Object)
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
            var response = await controller.CreateAsync(new CreateParentViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateParentResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateParentAsync(It.IsAny<CreateParentRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }
    }
}
