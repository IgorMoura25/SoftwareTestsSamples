using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;
using IntegrationManagerCallApiService;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Observability;

namespace IDSIM.Services.UnitTest.ApiGatewayTests
{
    [ExcludeFromCodeCoverage]
    public class IntegrationManagerWcfClientTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public IntegrationManagerWcfClientTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public async Task WebServiceCallAsync_ValidpDbID_ShouldExecuteSuccessfully()
        {
            // Arrange
            var log = new Mock<ILog>();
            var apiCall = new Mock<APICall>();
            var wcfClient = new IntegrationManagerWcfClient(log.Object, apiCall.Object);

            apiCall
                .Setup(c => c.webServiceCallAsync(It.IsAny<webServiceCallRequest>()))
                .Returns(Task.FromResult(_unitApiTestsFixture.GetValidWebServiceCallResponse()));

            // Act
            var response = await wcfClient.WebServiceCallAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.webServiceCallReturn);
        }

        [Fact]
        public void WebServiceCallAsync_InvalidpDbID_ShouldThrowException()
        {
            // Arrange
            var log = new Mock<ILog>();
            var apiCall = new Mock<APICall>();
            var wcfClient = new IntegrationManagerWcfClient(log.Object, apiCall.Object);

            apiCall
                .Setup(c => c.webServiceCallAsync(It.IsAny<webServiceCallRequest>()))
                .Throws(new Exception("com.idsgrp.ilconnect.pool.PoolException: Undefined is not a valid database pool name."));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await wcfClient.WebServiceCallAsync("Undefined", string.Empty));
        }

        [Fact]
        public void IntegrationManagerWcfClient_InvalidAppSettingsConstructor_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var log = new Mock<ILog>();
            Assert.ThrowsAny<Exception>(() => new IntegrationManagerWcfClient(log.Object));
        }
    }
}
