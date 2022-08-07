using POC.HelloWorld.API;
using POC.HelloWorld.API.Configuration;
using System;
using System.Net.Http;
using Xunit;

namespace POC.HelloWorld.IntegrationTests
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>>
    {
    }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly HelloWorldFactory<TStartup> Factory;
        public HttpClient Client;
        //public SoapSettings SoapSettings;

        public IntegrationTestsFixture()
        {
            Factory = new HelloWorldFactory<TStartup>();
            Client = Factory.CreateClient();
            //SoapSettings = TestSettings.GetSoapConfiguration();
        }

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }
    }
}
