using Xunit;
using TechTalk.SpecFlow;
using Automated_End2End_BDD.Tests.Fixtures;
using Automated_End2End_BDD.Tests.Pages;

namespace Automated_End2End_BDD.Tests.BDD
{
    [Binding]
    [Collection(nameof(MainFixtureCollection))]
    public class CommonStepsDefinitions
    {
        private readonly MainFixture _mainFixture;
        private readonly CatalogPage _catalogPage;

        public CommonStepsDefinitions(MainFixture mainFixture)
        {
            _mainFixture = mainFixture;
            _catalogPage = new CatalogPage(_mainFixture.SeleniumHelper);
        }

        [Given(@"the visitor is browsing the website")]
        public void GivenTheVisitorIsBrowsingTheWebsite()
        {
            // Arrange
            var expectedUrl = _mainFixture.SeleniumHelper.CombineWithDomainUrl(_mainFixture.Configuration.CatalogUrl);

            // Act
            _catalogPage.GoToCatalog();

            // Assert
            Assert.Contains(expectedUrl, _catalogPage.GetCurrentUrl());
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
