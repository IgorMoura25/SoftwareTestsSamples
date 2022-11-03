using Bogus;
using Automated_End2End_BDD.Tests.Models;

namespace Automated_End2End_BDD.Tests.Fixtures
{
    public class UserFixture
    {
        public User User;

        public UserFixture()
        {
            User = new User();
        }

        public void GenerateUser()
        {
            var faker = new Faker("pt_BR");

            User.Email = faker.Internet.Email().ToLower();
            User.Password = faker.Internet.Password(8, false, "", "@1Ab-");
        }
    }
}