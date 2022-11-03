using Xunit;
using TechTalk.SpecFlow;
using Automated_End2End_BDD.Tests.Fixtures;
using Automated_End2End_BDD.Tests.Pages;

namespace Automated_End2End_BDD.Tests.BDD.Account.StepDefinitions
{
    [Binding]
    [Collection(nameof(MainFixtureCollection))]
    public class User_RegistrationStepDefinitions
    {
        private readonly MainFixture _mainFixture;
        private readonly AccountRegisterPage _registerpage;

        public User_RegistrationStepDefinitions(MainFixture mainFixture)
        {
            _mainFixture = mainFixture;
            _registerpage = new AccountRegisterPage(_mainFixture.SeleniumHelper);
        }

        [When(@"he clicks in Create Your Account")]
        public void WhenHeClicksInCreateYourAccount()
        {
            // Arrange
            var expectedUrl = _mainFixture.SeleniumHelper.CombineWithDomainUrl(_mainFixture.Configuration.AccountRegisterUrl);

            // Act
            _registerpage.ClickCreateYourAccountButton();

            // Assert
            Assert.Contains(expectedUrl, _registerpage.GetCurrentUrl());
        }

        [When(@"fill the registration form with data")]
        public void WhenFillTheRegistrationFormWithData(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"click in Register button")]
        public void WhenClickInRegisterButton()
        {
            throw new PendingStepException();
        }

        [When(@"fill the registration form with data with a password without Upper Case letter")]
        public void WhenFillTheRegistrationFormWithDataWithAPasswordWithoutUpperCaseLetter(Table table)
        {
            throw new PendingStepException();
        }

        [Then(@"the Register button will not be able to be clicked")]
        public void ThenTheRegisterButtonWillNotBeAbleToBeClicked()
        {
            throw new PendingStepException();
        }

        [Then(@"an error message will appear showing that the password must contain an Upper Case letter")]
        public void ThenAnErrorMessageWillAppearShowingThatThePasswordMustContainAnUpperCaseLetter()
        {
            throw new PendingStepException();
        }

        [When(@"fill the registration form with data with a password without special character")]
        public void WhenFillTheRegistrationFormWithDataWithAPasswordWithoutSpecialCharacter(Table table)
        {
            throw new PendingStepException();
        }

        [Then(@"an error message will appear showing that the password must contain a special character")]
        public void ThenAnErrorMessageWillAppearShowingThatThePasswordMustContainASpecialCharacter()
        {
            throw new PendingStepException();
        }
    }
}
