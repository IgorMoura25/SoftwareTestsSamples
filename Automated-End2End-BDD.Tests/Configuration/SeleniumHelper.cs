using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Automated_End2End_BDD.Tests.Configuration
{
    /// <summary>
    /// É uma abstração do WebDriver do Selenium
    /// para melhor manutenção, tratativa de erros, clean code, etc
    /// </summary>
    public class SeleniumHelper : IDisposable
    {
        public readonly ConfigurationHelper Configuration;

        public readonly IWebDriver WebDriver;
        public WebDriverWait Wait;

        public SeleniumHelper(Browser browser, ConfigurationHelper configuration, bool headless = true)
        {
            Configuration = configuration;
            WebDriver = WebDriverFactory.CreateWebDriver(browser, Configuration.WebDriversPath, headless);

            // Para maximizar e ocupar a tela inteira
            WebDriver.Manage().Window.Maximize();

            // Um timeout padrão (30 segs)
            Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
        }

        #region Abstraindo e customizando os métodos do Selenium

        // Abstraindo e customizando os métodos do Selenium
        public string GetUrl()
        {
            return WebDriver.Url;
        }

        public string CombineWithDomainUrl(string path)
        {
            return $"{Configuration.DomainUrl}{path}";
        }

        public bool ContainsInUrl(string text)
        {
            return Wait.Until(ExpectedConditions.UrlContains(text));
        }

        public void GoToUrl(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public void ClickLinkByText(string linkText)
        {
            // Então aqui, ao invés de simplesmente buscar o elemento e dar um clique nele...
            // WebDriver.FindElement(By.LinkText(linkText)).Click();

            // Irá aguardar: até 30 segundos (configuração padrão do timeout do Wait)
            // até que: o elemento encontrado By.LinkText(x) esteja visível
            // para então: clicar nele
            Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(linkText)))
                .Click();
        }

        #region Element

        public IWebElement GetElementByClassName(string className)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(className)));
        }

        public IEnumerable<IWebElement> GetElementsByClassName(string className)
        {
            return Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));
        }

        public IWebElement GetElementByXPath(string xPath)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        }

        public IWebElement GetElementById(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
        }

        public string GetElementTextById(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Text;
        }

        public string GetElementValueById(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).GetAttribute("value");
        }

        public bool ElementExistsById(string id)
        {
            return ElementExists(By.Id(id));
        }

        #endregion

        #region Fill

        public void FillTextInputById(string id, string value)
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)))
                .SendKeys(value);
        }

        public void FillDropDownById(string id, string value)
        {
            var element = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));

            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }

        #endregion

        #region Clicks

        public void ClickById(string id)
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)))
                .Click();
        }

        public void ClickByXPath(string xPath)
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)))
                .Click();
        }

        #endregion

        #region Navigation

        public void ReturnNavigation(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; i++)
            {
                WebDriver.Navigate().Back();
            }
        }

        #endregion

        #region Screenshot

        public void GetScreenshot(string fileName)
        {
            SaveScreenshot(WebDriver.TakeScreenshot(), $"{DateTime.UtcNow.ToFileTimeUtc}_{fileName}.png");
        }

        #endregion

        private bool ElementExists(By by)
        {
            try
            {
                WebDriver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private void SaveScreenshot(Screenshot screenshot, string fileName)
        {
            screenshot.SaveAsFile($"{Configuration.ScreenshotsFolder}{fileName}", ScreenshotImageFormat.Png);
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                WebDriver.Quit();
                WebDriver.Dispose();
            }
        }
    }
}
