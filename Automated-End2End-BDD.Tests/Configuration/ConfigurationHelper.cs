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
        

        private readonly IConfiguration _configuration;

        public ConfigurationHelper()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
