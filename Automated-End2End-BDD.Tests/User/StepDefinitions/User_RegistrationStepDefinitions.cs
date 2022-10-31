using TechTalk.SpecFlow;

namespace Automated_End2End_BDD.Tests.User.StepDefinitions
{
    [Binding]
    public class User_RegistrationStepDefinitions
    {
        [Given(@"the visitor is browsing the website")]
        public void GivenTheVisitorIsBrowsingTheWebsite()
        {
            throw new PendingStepException();
        }

        [When(@"he clicks in Create Your Account")]
        public void WhenHeClicksInCreateYourAccount()
        {
            throw new PendingStepException();
        }

        [When(@"fill the form with data")]
        public void WhenFillTheFormWithData(Table table)
        {
            throw new PendingStepException();
        }

        [When(@"click in Register button")]
        public void WhenClickInRegisterButton()
        {
            throw new PendingStepException();
        }

        [Then(@"he will be redirected to the catalog")]
        public void ThenHeWillBeRedirectedToTheCatalog()
        {
            throw new PendingStepException();
        }

        [Then(@"his e-mail will appear in the top right menu")]
        public void ThenHisE_MailWillAppearInTheTopRightMenu()
        {
            throw new PendingStepException();
        }

        [When(@"fill the form with data with a password without Upper Case letter")]
        public void WhenFillTheFormWithDataWithAPasswordWithoutUpperCaseLetter(Table table)
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

        [When(@"fill the form with data with a password without special character")]
        public void WhenFillTheFormWithDataWithAPasswordWithoutSpecialCharacter(Table table)
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
