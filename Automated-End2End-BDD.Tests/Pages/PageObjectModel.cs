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

        public bool ValidateEmailInTheTopRightMenu(string email)
        {
            var text = Helper.GetElementTextByXPath("/html/body/app-root/app-navigation-header/header/nav/div/div/app-navigation-header-login/ul/li[1]/a");

            return text.Contains(email);
        }

        public bool ValidateInvalidPasswordErrorMessage()
        {
            var text = Helper.GetElementTextByXPath("/html/body/app-root/app-identity-root/app-identity-account/div/form/div[2]/div/span/p");

            return text.Contains("A senha precisa ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial");
        }

        public bool ValidateInvalidConfirmPasswordErrorMessage()
        {
            var text = Helper.GetElementTextByXPath("/html/body/app-root/app-identity-root/app-identity-account/div/form/div[3]/div/span/p");

            return text.Contains("A senha precisa ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial");
        }
    }
}
