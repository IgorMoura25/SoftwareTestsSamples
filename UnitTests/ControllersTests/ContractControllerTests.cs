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
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Contract;
using GetContractsByCCANRequestModel = Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract.GetContractsByCCANRequestModel;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Controllers;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Message;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class ContractControllerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public ContractControllerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task SearchAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validSearchRequestModel = _unitApiTestsFixture.GetValidSearchRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchContractsResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchAsync(validSearchRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchContractsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()), Times.Once);
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

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidSearchContractsResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchAsync(new SearchRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchContractsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()), Times.Once);
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

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.SearchAsync(new SearchRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchContractsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetDetailsAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validGetContractDetailsModel = _unitApiTestsFixture.GetValidGetContractDetailsModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractResponseModelFacadeResponse()));

            // Act
            var response = await controller.GetDetailsAsync(validGetContractDetailsModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractDetailsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractAsync(It.IsAny<GetContractRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task GetDetailsAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidGetContractResponseModelFacadeResponse()));

            // Act
            var response = await controller.GetDetailsAsync(new GetContractDetailsRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractDetailsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractAsync(It.IsAny<GetContractRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetDetailsAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.GetDetailsAsync(new GetContractDetailsRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractDetailsResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractAsync(It.IsAny<GetContractRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchByCustomerAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidSearchCustomerContractModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchContractsResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchByCustomerAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchCustomerContractResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task SearchByCustomerAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidSearchContractsResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchByCustomerAsync(new SearchByCustomerRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchCustomerContractResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchByCustomerAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.SearchByCustomerAsync(new SearchByCustomerRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchCustomerContractResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchContractsAsync(It.IsAny<SearchContractsRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetByCCANAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidApiGetContractsByCCANRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractsByCCANResponseModelFacadeResponse()));

            // Act
            var response = await controller.GetByCCANAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractsByCCANResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task GetByCCANAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidGetContractsByCCANResponseModelFacadeResponse()));

            // Act
            var response = await controller.GetByCCANAsync(new Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Contract.GetContractsByCCANRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractsByCCANResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetByCCANAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.GetByCCANAsync(new Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Contract.GetContractsByCCANRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractsByCCANResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidReserveContractNumberViewRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidReserveContractNumberResponseModelFacadeResponse()));

            // Act
            var response = await controller.ReserveContractNumberAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<ReserveContractNumberResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidReserveContractNumberResponseModelFacadeResponse()));

            // Act
            var response = await controller.ReserveContractNumberAsync(new ReserveContractNumberViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<ReserveContractNumberResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.ReserveContractNumberAsync(new ReserveContractNumberViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<ReserveContractNumberResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchMessagesAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validSearchMessagesRequestModel = _unitApiTestsFixture.GetValidSearchMessagesIMRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchMessagesResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchMessagesAsync(validSearchMessagesRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchMessagesResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task SearchMessagesAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidSearchMessagesResponseModelFacadeResponse()));

            // Act
            var response = await controller.SearchMessagesAsync(new SearchMessagesRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchMessagesResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchMessagesAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.SearchMessagesAsync(new SearchMessagesRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<SearchMessagesResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }


        [Fact]
        public async Task CreateMessageAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validCreateMessageRequestModel = _unitApiTestsFixture.GetValidCreateMessageRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateMessageResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateMessageAsync(false, validCreateMessageRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateMessageResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task CreateMessageAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidCreateMessageResponseModelFacadeResponse()));

            // Act
            var response = await controller.CreateMessageAsync(false, new CreateMessageRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateMessageResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateMessageAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.CreateMessageAsync(false, new CreateMessageRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<CreateMessageResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetLinkCodeDetailsByCCANAsync_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANViewRequestModel();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetValidRegion().ToString();

            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            dependency
                .Setup(c => c.IntegrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANResponseModelFacadeResponse()));

            // Act
            var response = await controller.GetLinkCodeDetailsByCCANAsync(validRequestModel);

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractLinkCodeDetailsByCCANResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal("OK", apiResponse?.ResponseCode);
            Assert.NotNull(apiResponse?.Response);
            Assert.Null(apiResponse?.Errors);
        }

        [Fact]
        public async Task GetLinkCodeDetailsByCCANAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Region"] = _unitApiTestsFixture.GetInvalidRegion().ToString();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
                .Setup(c => c.IntegrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetInvalidGetContractLinkCodeDetailsByCCANResponseModelFacadeResponse()));

            // Act
            var response = await controller.GetLinkCodeDetailsByCCANAsync(new GetContractLinkCodeDetailsByCCANViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractLinkCodeDetailsByCCANResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.Equal("BadRequest", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetLinkCodeDetailsByCCANAsync_FacadeReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var integrationManager = new Mock<IIntegrationManagerFacade>();
            var log = new Mock<ILog>();
            var dependency = new Mock<IControllerDependencyAggregate>();

            var controller = new ContractController(dependency.Object)
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
            var response = await controller.GetLinkCodeDetailsByCCANAsync(new GetContractLinkCodeDetailsByCCANViewRequestModel());

            // Assert
            var actionResult = (ObjectResult?)response.Result;
            var apiResponse = (ApiResponse<GetContractLinkCodeDetailsByCCANResponseModel>?)actionResult.Value;

            dependency.Verify(c => c.IntegrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()), Times.Once);
            Assert.Equal(actionResult?.StatusCode, (int)HttpStatusCode.InternalServerError);
            Assert.Equal("InternalServerError", apiResponse?.ResponseCode);
            Assert.Null(apiResponse?.Response);
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse?.Errors?.Count > 0);
        }
    }
}
