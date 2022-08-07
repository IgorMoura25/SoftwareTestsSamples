using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Services;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel;

namespace IDSIM.Services.UnitTest.ApiGatewayTests
{
    [ExcludeFromCodeCoverage]
    public class WebConnectServiceTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public WebConnectServiceTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task GetInfoLeaseVersionAsync_ValidRegion_ShouldReturnValidVersion()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidInfoLeaseVersionResponse()));

            // Act
            var response = await webConnectService.GetInfoLeaseVersionAsync(validRegion);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Parameter?.ParamILSubRelease != null);
            Assert.True(response.Body?.Response?.Parameter?.ParamSysRelease != null);
        }

        [Fact]
        public void GetInfoLeaseVersionAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRegion = _unitApiTestsFixture.GetInvalidRegion();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.GetInfoLeaseVersionAsync(invalidRegion));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SearchContractsAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validSearchContractsRequestModel = _unitApiTestsFixture.GetValidSearchContractsRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchContractsResponse()));

            // Act
            var response = await webConnectService.SearchContractsAsync(validSearchContractsRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void SearchContractsAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidSearchContractsRequestModel = _unitApiTestsFixture.GetInvalidSearchContractsRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.SearchContractsAsync(invalidSearchContractsRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SearchAssetsAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validSearchAssetsRequestModel = _unitApiTestsFixture.GetValidSearchAssetsRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchAssetsResponse()));

            // Act
            var response = await webConnectService.SearchAssetsAsync(validSearchAssetsRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void SearchAssetsAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidSearchAssetsRequestModel = _unitApiTestsFixture.GetInvalidSearchAssetsRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.SearchAssetsAsync(invalidSearchAssetsRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SearchInvoicesAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validSearchInvoicesRequestModel = _unitApiTestsFixture.GetValidSearchInvoicesRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchInvoicesResponse()));

            // Act
            var response = await webConnectService.SearchInvoicesAsync(validSearchInvoicesRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void SearchInvoicesAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidSearchInvoicesRequestModel = _unitApiTestsFixture.GetInvalidSearchInvoicesRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.SearchInvoicesAsync(invalidSearchInvoicesRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetContractAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validGetContractRequestModel = _unitApiTestsFixture.GetValidGetContractRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractResponse()));

            // Act
            var response = await webConnectService.GetContractAsync(validGetContractRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void GetContractAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidGetContractRequestModel = _unitApiTestsFixture.GetInvalidGetContractRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.GetContractAsync(invalidGetContractRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePromiseToPayDateAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidUpdatePromiseToPayDateRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidUpdatePromiseToPayDateResponse()));

            // Act
            var response = await webConnectService.UpdatePromiseToPayDateAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void UpdatePromiseToPayDateAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidUpdatePromiseToPayDateRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.UpdatePromiseToPayDateAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ListParentsAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidListParentsRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidListParentsResponse()));

            // Act
            var response = await webConnectService.ListParentsAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.ParentCode != null);
            Assert.True(response.Body?.Response?.ParentCode?.Length > 0);
        }

        [Fact]
        public void ListParentsAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidListParentsRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.ListParentsAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetContractsByCCANAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRegion = _unitApiTestsFixture.GetValidRegion();
            var validRequestModel = _unitApiTestsFixture.GetValidApiGatewayGetContractsByCCANRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractsByCCANResponse()));

            // Act
            var response = await webConnectService.GetContractsByCCANAsync(validRequestModel);

            // Assert
            Assert.True(validRegion.IsActive);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void GetContractsByCCANAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidApiGatewayGetContractsByCCANRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.GetContractsByCCANAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateParentAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidCreateParentRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateParentResponse()));

            // Act
            var response = await webConnectService.CreateParentAsync(validRequestModel);

            // Assert
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void CreateParentAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidCreateParentRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.CreateParentAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ReserveContractNumberAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidReserveContractNumberRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidReserveContractNumberResponse()));

            // Act
            var response = await webConnectService.ReserveContractNumberAsync(validRequestModel);

            // Assert
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void ReserveContractNumberAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidReserveContractNumberRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.ReserveContractNumberAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SearchDealerAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidSearchDealerRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidSearchDealerResponse()));

            // Act
            var response = await webConnectService.SearchDealerAsync(validRequestModel);

            // Assert
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void SearchDealerAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidSearchDealerRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.SearchDealerAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateDealerAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidCreateDealerRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidCreateDealerResponse()));

            // Act
            var response = await webConnectService.CreateDealerAsync(validRequestModel);

            // Assert
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void CreateDealerAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidCreateDealerRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.CreateDealerAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetContractLinkCodeDetailsByCCANAsync_ValidRegion_ShouldExecuteSuccessfully()
        {
            // Arrange
            var validRequestModel = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANResponse()));

            // Act
            var response = await webConnectService.GetContractLinkCodeDetailsByCCANAsync(validRequestModel);

            // Assert
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(response.Body?.Response?.Customer != null);
            Assert.True(response.Body?.Response?.Customer?.CsKey != null);
        }

        [Fact]
        public void GetContractLinkCodeDetailsByCCANAsync_InvalidRegion_ShouldThrowException()
        {
            // Arrange
            var invalidRequestModel = _unitApiTestsFixture.GetInvalidGetContractLinkCodeDetailsByCCANRequestModel();
            var wcfClient = new Mock<IIntegrationManagerWcfClient>();

            var webConnectService = new WebConnectService(wcfClient.Object);

            wcfClient
                .Setup(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("is not a valid database pool name"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await webConnectService.GetContractLinkCodeDetailsByCCANAsync(invalidRequestModel));
            Assert.Contains("is not a valid database pool name", exception?.Result?.Message);
            wcfClient.Verify(c => c.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
