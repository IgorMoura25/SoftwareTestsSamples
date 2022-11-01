using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Automated_End2End_BDD.Tests.Configuration
{
    /// <summary>
    /// Diz quais browsers a automação está suportando
    /// </summary>
    public enum Browser
    {
        Chrome = 1
    }

    /// <summary>
    /// Para construir o WebDriver do Selenium  
    /// </summary>
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(Browser browser, string driverPath, bool headless)
        {
            // Headless: navegar pelo chrome sem abrir uma janela, como se fosse por baixo dos panos

            IWebDriver? webDriver = null;

            switch (browser)
            {
                case Browser.Chrome:
                    var options = new ChromeOptions();

                    if (headless) options.AddArgument("--headless");

                    webDriver = new ChromeDriver(driverPath, options);

                    break;
            }

            if (webDriver == null) throw new NullReferenceException("Browser not supported for creating WebDriver");

            return webDriver;
        }
    }
}
