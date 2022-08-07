using POC.HelloWorld.API;
using System.Threading.Tasks;
using Xunit;

namespace POC.HelloWorld.IntegrationTests.EchoHandler
{
    [Trait("Integration", "Echo Handler REST Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class EchoHandlerRestTests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;

        public EchoHandlerRestTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Echo String")]
        public async Task Echo_AnyTextAsInput_ReturnsSameText()
        {
            //Arrange & Act
            var echoResponse = await _testsFixture.Client.GetAsync($"api/v1/echo?input=test&region=US");
            var responseText = await echoResponse.Content.ReadAsStringAsync();

            //Assert
            echoResponse.EnsureSuccessStatusCode();
            Assert.Contains("test", responseText);
        }
    }
}
