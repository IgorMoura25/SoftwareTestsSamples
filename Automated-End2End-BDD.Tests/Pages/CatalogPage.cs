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
    }
}
