using Xunit;
using Automated_End2End_BDD.Tests.Configuration;


namespace Automated_End2End_BDD.Tests.Fixtures
{
    [CollectionDefinition(nameof(MainFixtureCollection))]
    public class MainFixtureCollection : ICollectionFixture<MainFixture>
    {
    }

    public class MainFixture
    {
        public readonly SeleniumHelper SeleniumHelper;
        public readonly ConfigurationHelper Configuration;

        public MainFixture()
        {
            Configuration = new ConfigurationHelper();
            SeleniumHelper = new SeleniumHelper(Browser.Chrome, Configuration, false);
        }
    }
}
