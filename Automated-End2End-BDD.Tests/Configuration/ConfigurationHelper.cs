using Microsoft.Extensions.Configuration;

namespace Automated_End2End_BDD.Tests.Configuration
{
    /// <summary>
    /// Para auxiliar a busca por variáveis globais do appsettings
    /// evitando código fixo, necessário instalação do Microsoft.Extensions.Configuration.Json
    /// </summary>
    public class ConfigurationHelper
    {
        public string WebDriversPath => $"{_configuration.GetSection("WebDrivers").Value}";
        public string ScreenshotsFolder => $"{_configuration.GetSection("ScreenshotsFolder").Value}";
        public string DomainUrl => $"{_configuration.GetSection("DomainUrl").Value}";
        public string CatalogUrl => $"{_configuration.GetSection("CatalogUrl").Value}";
        public string AccountRegisterUrl => $"{_configuration.GetSection("AccountRegisterUrl").Value}";


        private readonly IConfiguration _configuration;

        public ConfigurationHelper()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
