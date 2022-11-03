using Automated_End2End_BDD.Tests.Configuration;

namespace Automated_End2End_BDD.Tests.Pages
{
    public class CatalogPage : PageObjectModel
    {
        public CatalogPage(SeleniumHelper helper) : base(helper) { }

        public void GoToCatalog()
        {
            Helper.GoToUrl(Helper.CombineWithDomainUrl(Helper.Configuration.CatalogUrl));
        }

        public bool IsCatalogPage()
        {
            var text = Helper.GetElementTextByXPath("/html/body/app-root/app-navigation-catalog/header/div/div/div[2]/h1");

            return text.Contains("Desenvolvimento Avançado em Angular");
        }
    }
}
