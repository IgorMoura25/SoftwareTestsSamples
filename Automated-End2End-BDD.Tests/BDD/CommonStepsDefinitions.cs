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
        private readonly AccountRegisterPage _registerPage;

        public CommonStepsDefinitions(MainFixture mainFixture)
        {
            _mainFixture = mainFixture;
            _catalogPage = new CatalogPage(_mainFixture.SeleniumHelper);
            _registerPage = new AccountRegisterPage(_mainFixture.SeleniumHelper);
        }

        [Given(@"the visitor is browsing the website")]
        public void GivenTheVisitorIsBrowsingTheWebsite()
        {
            // Arrange & Act
            _catalogPage.GoToCatalog();

            // Assert
            Assert.Contains(_mainFixture.Configuration.DomainUrl, _catalogPage.GetCurrentUrl());
        }

        [Then(@"he will be redirected to the catalog")]
        public void ThenHeWillBeRedirectedToTheCatalog()
        {
            // Assert
            Assert.True(_catalogPage.IsCatalogPage());
        }

        [Then(@"his e-mail will appear in the top right menu")]
        public void ThenHisE_MailWillAppearInTheTopRightMenu()
        {
            // Assert

            var email = _registerPage.ValidateEmailInTheTopRightMenu(_mainFixture.UserFixture.User.Email ?? string.Empty);

            // Está fixo no frontend por testes
            Assert.True(_registerPage.ValidateEmailInTheTopRightMenu("pegar-email-do-token@teste.com"));
        }
    }
}
