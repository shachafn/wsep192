using DomainLayer;
using NUnit.Framework;
using System.Collections.Generic;
using static DomainLayer.ShopOwner;

namespace UnitTests
{
    [TestFixture]
    class ShopOwnerTest
    {
        User meni;
        Shop menisShop;
        [SetUp]
        public void Setup()
        {
            ShopOwner.CleanDict();
            User.users = new Dictionary<string, User>();
            meni = new User("meni", "123456");
            meni.Login("meni", "123456");
            menisShop = new Shop();
            ShopOwner.NewShopOwner(meni, menisShop);
        }
        [Test]
        public void TestNewShopOwner()
        {
            Assert.NotNull(ShopOwner.GetShopOwner(meni, menisShop));
        }

        [Test]
        public void TestAddManager()
        {
            ShopOwner meniOwner = ShopOwner.GetShopOwner(meni, menisShop);
            User beni = new User("beni","123456");
            //add a new manager with an adding privelage
            List<string> actionsAllowed = new List<string>
            {
                "addManager"
            };
            ManagingPrivileges privs = new ManagingPrivileges(actionsAllowed);
            Assert.True(meniOwner.AddManager(beni,privs));
            Assert.NotNull(ShopOwner.GetShopOwner(beni, menisShop));
            ShopOwner beniOwner = ShopOwner.GetShopOwner(beni, menisShop);
            Assert.False(beniOwner.CloseShop());
        }
    }
}
