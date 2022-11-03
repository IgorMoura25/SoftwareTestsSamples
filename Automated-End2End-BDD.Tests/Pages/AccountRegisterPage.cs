using Automated_End2End_BDD.Tests.Configuration;
using Automated_End2End_BDD.Tests.Models;

namespace Automated_End2End_BDD.Tests.Pages
{
    public class AccountRegisterPage : PageObjectModel
    {
        public AccountRegisterPage(SeleniumHelper helper) : base(helper) { }

        public void ClickCreateYourAccountButton()
        {
            Helper.GetElementByXPath("/html/body/app-root/app-navigation-header/header/nav/div/div/app-navigation-header-login/ul/li[2]/a").Click();
        }

        public void FillRegisterForm(RegisterFormModel form)
        {
            Helper.FillTextInputById("email", form.Email);
            Helper.FillTextInputById("password", form.Password);
            Helper.FillTextInputById("passwordConfirmation", form.ConfirmPassword);
        }

        public bool ValidateFillRegisterForm(RegisterFormModel form)
        {
            if (Helper.GetElementTextById("email") != form.Email) return false;
            if (Helper.GetElementTextById("password") != form.Password) return false;
            if (Helper.GetElementTextById("passwordConfirmation") != form.ConfirmPassword) return false;

            return true;
        }

        public void ClickRegisterButton()
        {
            Helper.GetElementById("register").Click();
        }
    }
}
