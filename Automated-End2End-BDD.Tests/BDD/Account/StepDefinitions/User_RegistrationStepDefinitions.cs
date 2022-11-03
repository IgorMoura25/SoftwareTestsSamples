using Xunit;
using TechTalk.SpecFlow;
using Automated_End2End_BDD.Tests.Fixtures;
using Automated_End2End_BDD.Tests.Pages;
using Automated_End2End_BDD.Tests.Models;

namespace Automated_End2End_BDD.Tests.BDD.Account.StepDefinitions
{
    [Binding]
    [Collection(nameof(MainFixtureCollection))]
    public class User_RegistrationStepDefinitions
    {
        private readonly MainFixture _mainFixture;
        private readonly AccountRegisterPage _registerPage;

        public User_RegistrationStepDefinitions(MainFixture mainFixture)
        {
            _mainFixture = mainFixture;
            _registerPage = new AccountRegisterPage(_mainFixture.SeleniumHelper);
        }

        [When(@"he clicks in Create Your Account")]
        public void WhenHeClicksInCreateYourAccount()
        {
            // Act
            _registerPage.ClickCreateYourAccountButton();

            // Assert
            Assert.True(_registerPage.IsRegisterPage());
        }

        [When(@"fill the registration form with data")]
        public void WhenFillTheRegistrationFormWithData(Table table)
        {
            // Arrange
            _mainFixture.UserFixture.GenerateUser();
            var registerForm = new RegisterFormModel()
            {
                Email = _mainFixture.UserFixture.User.Email,
                Password = _mainFixture.UserFixture.User.Password,
                ConfirmPassword = _mainFixture.UserFixture.User.Password
            };

            // Act
            _registerPage.FillRegisterForm(registerForm);

            // Assert
            _registerPage.ValidateFillRegisterForm(registerForm);
        }

        [When(@"click in Register button")]
        public void WhenClickInRegisterButton()
        {
            _registerPage.ClickRegisterButton();
        }

        [When(@"fill the registration form with data with a password without Upper Case letter")]
        public void WhenFillTheRegistrationFormWithDataWithAPasswordWithoutUpperCaseLetter(Table table)
        {
            // Arrange
            _mainFixture.UserFixture.GenerateUser();
            var user = _mainFixture.UserFixture.User;
            user.Password = "senha-2501";

            var registerForm = new RegisterFormModel()
            {
                Email = user.Email,
                Password = user.Password,
                ConfirmPassword = user.Password
            };

            // Act
            _registerPage.FillRegisterForm(registerForm);

            // Assert
            _registerPage.ValidateFillRegisterForm(registerForm);
        }

        [Then(@"the Register button will not be able to be clicked")]
        public void ThenTheRegisterButtonWillNotBeAbleToBeClicked()
        {
            // Assert
            Assert.False(_registerPage.CanClickRegisterButton());
        }

        [Then(@"an error message will appear showing that the password must contain an Upper Case letter")]
        public void ThenAnErrorMessageWillAppearShowingThatThePasswordMustContainAnUpperCaseLetter()
        {
            // Assert
            Assert.True(_registerPage.ValidateInvalidPasswordErrorMessage());
        }

        [When(@"fill the registration form with data with a password without special character")]
        public void WhenFillTheRegistrationFormWithDataWithAPasswordWithoutSpecialCharacter(Table table)
        {
            // Arrange
            _mainFixture.UserFixture.GenerateUser();
            var user = _mainFixture.UserFixture.User;
            user.Password = "Senha2501";

            var registerForm = new RegisterFormModel()
            {
                Email = user.Email,
                Password = user.Password,
                ConfirmPassword = user.Password
            };

            // Act
            _registerPage.FillRegisterForm(registerForm);

            // Assert
            _registerPage.ValidateFillRegisterForm(registerForm);
        }

        [Then(@"an error message will appear showing that the password must contain a special character")]
        public void ThenAnErrorMessageWillAppearShowingThatThePasswordMustContainASpecialCharacter()
        {
            // Assert
            Assert.True(_registerPage.ValidateInvalidPasswordErrorMessage());
            Assert.True(_registerPage.ValidateInvalidConfirmPasswordErrorMessage());
        }
    }
}
