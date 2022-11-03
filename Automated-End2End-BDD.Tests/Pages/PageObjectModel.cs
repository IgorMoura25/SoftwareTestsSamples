using Automated_End2End_BDD.Tests.Configuration;

namespace Automated_End2End_BDD.Tests.Pages
{
    public abstract class PageObjectModel
    {
        protected readonly SeleniumHelper Helper;

        public PageObjectModel(SeleniumHelper helper)
        {
            Helper = helper;
        }

        // Implementar aqui métodos que são genéricos e que possam ser
        // reutilizados por todas as PageObject filhas...

        public string GetCurrentUrl()
        {
            return Helper.GetUrl();
        }
    }
}
