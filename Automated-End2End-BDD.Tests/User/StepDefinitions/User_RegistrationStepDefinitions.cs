using System;
using TechTalk.SpecFlow;

namespace Automated_End2End_BDD.Tests.User.StepDefinitions
{
    [Binding]
    public class User_RegistrationStepDefinitions
    {
        [When(@"he clicks in Create Your Account")]
        public void WhenHeClicksInCreateYourAccount()
        {
            throw new PendingStepException();
        }

        [When(@"fill the registration form with data")]
        public void WhenFillTheRegistrationFormWithData(Table table)
        {
            throw new PendingStepException();
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
