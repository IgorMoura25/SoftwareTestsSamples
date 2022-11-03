using Xunit;
using TechTalk.SpecFlow;
using Automated_End2End_BDD.Tests.Fixtures;
using Automated_End2End_BDD.Tests.Pages;
using Automated_End2End_BDD.Tests.Models;

namespace Automated_End2End_BDD.Tests.BDD.Account.StepDefinitions
{
    [Binding]
    [Collection(nameof(MainFixtureCollection))]
    public class User_LoginStepDefinitions
    {
        private readonly MainFixture _mainFixture;
        private readonly AccountLoginPage _loginPage;

        public User_LoginStepDefinitions(MainFixture mainFixture)
        {
            _mainFixture = mainFixture;
            _loginPage = new AccountLoginPage(_mainFixture.SeleniumHelper);
        }

        [When(@"he clicks in Enter")]
        public void WhenHeClicksInEnter()
        {
            // Act
            _loginPage.ClickEnterButton();

            // Assert
            Assert.True(_loginPage.IsLoginPage());
        }

        [When(@"fill the login form with data")]
        public void WhenFillTheLoginFormWithData(Table table)
        {
            // Arrange
            var registerForm = new LoginFormModel()
            {
                Email = "tester@test.com",
                Password = "Teste-1234"
            };

            // Act
            _loginPage.FillLoginForm(registerForm);

            // Assert
            _loginPage.ValidateFillLoginForm(registerForm);
        }

        [When(@"click in Enter button")]
        public void WhenClickInEnterButton()
        {
            // Act
            _loginPage.ClickSubmitEnterButton();
        }
    }
}
