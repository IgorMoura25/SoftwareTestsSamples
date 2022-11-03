using Automated_End2End_BDD.Tests.Configuration;
using Automated_End2End_BDD.Tests.Models;

namespace Automated_End2End_BDD.Tests.Pages
{
    public class AccountLoginPage : PageObjectModel
    {
        public AccountLoginPage(SeleniumHelper helper) : base(helper) { }

        public void ClickEnterButton()
        {
            Helper.GetElementByXPath("/html/body/app-root/app-navigation-header/header/nav/div/div/app-navigation-header-login/ul/li[1]/a").Click();
        }

        public bool IsLoginPage()
        {
            var text = Helper.GetElementTextByXPath("/html/body/app-root/app-identity-root/app-identity-login/div/h1");

            return text.Contains("Entrar");
        }

        public void FillLoginForm(LoginFormModel form)
        {
            Helper.FillTextInputById("email", form.Email);
            Helper.FillTextInputById("password", form.Password);
        }

        public bool ValidateFillLoginForm(LoginFormModel form)
        {
            if (Helper.GetElementTextById("email") != form.Email) return false;
            if (Helper.GetElementTextById("password") != form.Password) return false;

            return true;
        }

        public void ClickSubmitEnterButton()
        {
            Helper.GetElementById("login").Click();
        }
    }
}
