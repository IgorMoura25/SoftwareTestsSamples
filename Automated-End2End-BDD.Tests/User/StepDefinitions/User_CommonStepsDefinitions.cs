using Xunit;
using TechTalk.SpecFlow;
using Automated_End2End_BDD.Tests.Fixtures;

namespace Automated_End2End_BDD.Tests.User.StepDefinitions
{
    [Binding]
    [Collection(nameof(MainFixtureCollection))]
    public class User_CommonStepsDefinitions
    {
        private readonly MainFixture _mainFixture;

        public User_CommonStepsDefinitions(MainFixture mainFixture)
        {
            _mainFixture = mainFixture;
        }

        [Given(@"the visitor is browsing the website")]
        public void GivenTheVisitorIsBrowsingTheWebsite()
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
    }
}
