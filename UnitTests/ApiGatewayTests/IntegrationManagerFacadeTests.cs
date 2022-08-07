using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Services;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Invoice;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Parent;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Dealer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Message;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ApiGatewayTests
{
    [ExcludeFromCodeCoverage]
    public class IntegrationManagerFacadeTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public IntegrationManagerFacadeTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task GetIntegrationManagerHealthCheckAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetInfoLeaseVersionAsync(validRegion))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidInfoLeaseVersionDto()));

            // Act
            var response = await integrationManagerFacade.GetIntegrationManagerHealthCheckAsync(validRegion);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.GetInfoLeaseVersionAsync(validRegion), Times.Once);
            Assert.Equal("UP", response.Status);
            Assert.Null(response.Details);
        }

        [Fact]
        public async Task GetIntegrationManagerHealthCheckAsync_NullRegion_ShouldReturnError()
        {
            // Arrange
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            // Act
            var response = await integrationManagerFacade.GetIntegrationManagerHealthCheckAsync(null);

            // Assert
            webConnect.Verify(c => c.GetInfoLeaseVersionAsync(null), Times.Never);
            Assert.Equal("error", response.Status);
        }

        [Fact]
        public async Task GetIntegrationManagerHealthCheckAsync_FaultResponse_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetInfoLeaseVersionAsync(validRegion))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetFaultedInfoLeaseVersionDto()));

            // Act
            var response = await integrationManagerFacade.GetIntegrationManagerHealthCheckAsync(validRegion);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.GetInfoLeaseVersionAsync(validRegion), Times.Once);
            Assert.Equal("UP", response.Status);
            Assert.Equal("il9-down", response.Details);
        }

        [Fact]
        public async Task GetIntegrationManagerHealthCheckAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetInfoLeaseVersionAsync(invalidRegion))
                .Throws(new Exception("is not a valid database pool name"));

            // Act
            var response = await integrationManagerFacade.GetIntegrationManagerHealthCheckAsync(invalidRegion);

            // Assert
            webConnect.Verify(c => c.GetInfoLeaseVersionAsync(null), Times.Never);
            Assert.Equal("error", response.Status);
        }

        [Fact]
        public async Task SearchContractsAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidSearchContractsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchContractsAsync(validRequestModel))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchContractsResponseDto()));

            // Act
            var response = await integrationManagerFacade.SearchContractsAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.SearchContractsAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.True(response.Data?.SearchResult?.Count > 0);
        }

        [Fact]
        public async Task SearchContractsAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidSearchContractsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.SearchContractsAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.SearchContractsAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.SearchContractsAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchContractsAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidSearchContractsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchContractsAsync(requestModel))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.SearchContractsAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.SearchContractsAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchAssetsAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidSearchAssetsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchAssetsAsync(validRequestModel))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchAssetsResponseDto()));

            // Act
            var response = await integrationManagerFacade.SearchAssetsAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.SearchAssetsAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.True(response.Data?.ContractAssets?.Count > 0);
        }

        [Fact]
        public async Task SearchAssetsAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidSearchAssetsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.SearchAssetsAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.SearchAssetsAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.SearchAssetsAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchAssetsAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidSearchAssetsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchAssetsAsync(requestModel))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.SearchAssetsAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.SearchAssetsAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchInvoicesAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidSearchInvoicesRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractResponseDto()));

            webConnect
                .Setup(c => c.SearchInvoicesAsync(validRequestModel))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchInvoicesResponseDto()));

            // Act
            var response = await integrationManagerFacade.SearchInvoicesAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.SearchInvoicesAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.Contract);
            Assert.True(response.Data?.Invoice?.Count > 0);
        }

        [Fact]
        public async Task SearchInvoicesAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidSearchInvoicesRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.SearchInvoicesAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.SearchInvoicesAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.SearchInvoicesAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchInvoicesAsync_ResponseFromGetContractIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var getContractRequestModel = _unitApiTestsFixture.GetValidGetContractRequestModel();
            var requestModel = _unitApiTestsFixture.GetValidSearchInvoicesRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.SearchInvoicesAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.SearchInvoicesAsync(requestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchInvoicesAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidSearchInvoicesRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractResponseDto()));

            webConnect
                .Setup(c => c.SearchInvoicesAsync(It.IsAny<SearchInvoicesRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.SearchInvoicesAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.GetContractAsync(It.IsAny<GetContractRequestModel>()), Times.Once);
            webConnect.Verify(c => c.SearchInvoicesAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidGetContractRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractResponseDto()));

            // Act
            var response = await integrationManagerFacade.GetContractAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.GetContractAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.Contract);
        }

        [Fact]
        public async Task GetContractAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidGetContractRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.GetContractAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.GetContractAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.GetContractAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidGetContractRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractAsync(It.IsAny<GetContractRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.GetContractAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.GetContractAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task UpdatePromiseToPayDateAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidUpdatePromiseToPayDateRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidUpdatePromiseToPayDateResponseDto()));

            // Act
            var response = await integrationManagerFacade.UpdatePromiseToPayDateAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.UpdatePromiseToPayDateAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(0, response.Data.StatusCode);
        }

        [Fact]
        public async Task UpdatePromiseToPayDateAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidUpdatePromiseToPayDateRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.UpdatePromiseToPayDateAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.UpdatePromiseToPayDateAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.UpdatePromiseToPayDateAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task UpdatePromiseToPayDateAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidUpdatePromiseToPayDateRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.UpdatePromiseToPayDateAsync(It.IsAny<UpdatePromiseToPayDateRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.UpdatePromiseToPayDateAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.UpdatePromiseToPayDateAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task ListParentsAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidListParentsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.ListParentsAsync(validRequestModel))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidListParentsResponseDto()));

            // Act
            var response = await integrationManagerFacade.ListParentsAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.ListParentsAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.True(response.Data?.ParentCodeList?.Count > 0);
        }

        [Fact]
        public async Task ListParentsAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidListParentsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.ListParentsAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.ListParentsAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.ListParentsAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task ListParentsAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidListParentsRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.ListParentsAsync(It.IsAny<ListParentsRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.ListParentsAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.ListParentsAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractsByCCANAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidApiGatewayGetContractsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractsByCCANResponseDto()));

            // Act
            var response = await integrationManagerFacade.GetContractsByCCANAsync(validRequestModel);

            // Assert
            webConnect.Verify(c => c.GetContractsByCCANAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.StatusCode);
        }

        [Fact]
        public async Task GetContractsByCCANAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidApiGatewayGetContractsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.GetContractsByCCANAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.GetContractsByCCANAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.GetContractsByCCANAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractsByCCANAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidApiGatewayGetContractsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractsByCCANAsync(It.IsAny<GetContractsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.GetContractsByCCANAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.GetContractsByCCANAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateParentAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidCreateParentRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateParentAsync(It.IsAny<CreateParentRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateParentResponseDto()));

            // Act
            var response = await integrationManagerFacade.CreateParentAsync(validRequestModel);

            // Assert
            webConnect.Verify(c => c.CreateParentAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(0, response.Data.StatusCode);
        }

        [Fact]
        public async Task CreateParentAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidCreateParentRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.CreateParentAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.CreateParentAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.CreateParentAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateParentAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidCreateParentRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateParentAsync(It.IsAny<CreateParentRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.CreateParentAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.CreateParentAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateParentAsync_ResponseIsError_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetErrorCreateParentRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateParentAsync(It.IsAny<CreateParentRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetErrorCreateParentResponseDto()));

            // Act
            var response = await integrationManagerFacade.CreateParentAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.CreateParentAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidReserveContractNumberRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidReserveContractNumberResponseDto()));

            // Act
            var response = await integrationManagerFacade.ReserveContractNumberAsync(validRequestModel);

            // Assert
            webConnect.Verify(c => c.ReserveContractNumberAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(0, response.Data.StatusCode);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidReserveContractNumberRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.ReserveContractNumberAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.ReserveContractNumberAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.ReserveContractNumberAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidReserveContractNumberRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.ReserveContractNumberAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.ReserveContractNumberAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_ResponseIsError_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetErrorReserveContractNumberRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.ReserveContractNumberAsync(It.IsAny<ReserveContractNumberRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetErrorReserveContractNumberResponseDto()));

            // Act
            var response = await integrationManagerFacade.ReserveContractNumberAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.ReserveContractNumberAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchDealerAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidSearchDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchDealerResponseDto()));

            // Act
            var response = await integrationManagerFacade.SearchDealerAsync(validRequestModel);

            // Assert
            webConnect.Verify(c => c.SearchDealerAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.StatusCode);
        }

        [Fact]
        public async Task SearchDealerAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidSearchDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.SearchDealerAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.SearchDealerAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.SearchDealerAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchDealerAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidSearchDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.SearchDealerAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.SearchDealerAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchDealerAsync_ResponseIsError_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidSearchDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchDealerAsync(It.IsAny<SearchDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetErrorSearchDealerResponseDto()));

            // Act
            var response = await integrationManagerFacade.SearchDealerAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.SearchDealerAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateDealerAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidCreateDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateDealerResponseDto()));

            // Act
            var response = await integrationManagerFacade.CreateDealerAsync(validRequestModel);

            // Assert
            webConnect.Verify(c => c.CreateDealerAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(0, response.Data.StatusCode);
        }

        [Fact]
        public async Task CreateDealerAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidCreateDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.CreateDealerAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.CreateDealerAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.CreateDealerAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateDealerAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidCreateDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.CreateDealerAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.CreateDealerAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateDealerAsync_ResponseIsError_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidCreateDealerRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateDealerAsync(It.IsAny<CreateDealerRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetErrorCreateDealerResponseDto()));

            // Act
            var response = await integrationManagerFacade.CreateDealerAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.CreateDealerAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchMessagesAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidSearchMessagesIMRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchMessagesResponseDto()));

            // Act
            var response = await integrationManagerFacade.SearchMessagesAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.SearchMessagesAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal("0", response.Data.MessageStatus.code);
        }

        [Fact]
        public async Task SearchMessagesAsyncAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange           
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidSearchMessagesIMRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.SearchMessagesAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.SearchMessagesAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.SearchMessagesAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task SearchMessagesAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidSearchMessagesIMRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.SearchMessagesAsync(It.IsAny<SearchMessagesIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.SearchMessagesAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.SearchMessagesAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateMessageAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidCreateMessageIMRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateMessageResponseDto()));

            // Act
            var response = await integrationManagerFacade.CreateMessageAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            webConnect.Verify(c => c.CreateMessageAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal("0", response.Data.code);
        }

        [Fact]
        public async Task CreateMessageAsyncAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange           
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidCreateMessageIMRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.CreateMessageAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.CreateMessageAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.CreateMessageAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task CreateMessageAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidCreateMessageIMRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.CreateMessageAsync(It.IsAny<CreateMessageIMRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.CreateMessageAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.CreateMessageAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractLinkCodeDetailsByCCANAsync_ValidRegion_ShouldReturnSuccess()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANResponseDto()));

            // Act
            var response = await integrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(validRequestModel);

            // Assert
            webConnect.Verify(c => c.GetContractLinkCodeDetailsByCCANAsync(validRequestModel), Times.Once);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.StatusCode);
        }

        [Fact]
        public async Task GetContractLinkCodeDetailsByCCANAsync_InvalidRegion_ShouldReturnError()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidGetContractLinkCodeDetailsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect.Setup(c => c.GetContractLinkCodeDetailsByCCANAsync(invalidRequestModel));

            // Act
            var response = await integrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(invalidRequestModel);

            // Assert
            webConnect.Verify(c => c.GetContractLinkCodeDetailsByCCANAsync(invalidRequestModel), Times.Never);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractLinkCodeDetailsByCCANAsync_ResponseIsFaultBody_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidFaultReturnDataDto()));

            // Act
            var response = await integrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.GetContractLinkCodeDetailsByCCANAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }

        [Fact]
        public async Task GetContractLinkCodeDetailsByCCANAsync_ResponseIsError_ShouldReturnError()
        {
            // Arrange
            var requestModel = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANRequestModel();
            var webConnect = new Mock<IWebConnectService>();
            var log = new Mock<ILog>();
            var integrationManagerFacade = new IntegrationManagerFacade(webConnect.Object, log.Object);

            webConnect
                .Setup(c => c.GetContractLinkCodeDetailsByCCANAsync(It.IsAny<GetContractLinkCodeDetailsByCCANRequestModel>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetErrorGetContractLinkCodeDetailsByCCANResponseDto()));

            // Act
            var response = await integrationManagerFacade.GetContractLinkCodeDetailsByCCANAsync(requestModel);

            // Assert
            webConnect.Verify(c => c.GetContractLinkCodeDetailsByCCANAsync(requestModel), Times.Once);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors?.Count > 0);
        }
    }
}
