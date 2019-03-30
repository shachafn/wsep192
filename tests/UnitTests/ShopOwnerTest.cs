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
            ShopOwner.cleanDict();
            User.users = new Dictionary<string, User>();
            User meni = new User("meni", "123456");
            meni.Login("meni", "123456");
            Shop menisShop = new Shop();
            ShopOwner.NewShopOwner(meni, menisShop);
        }
        [Test]
        public void testNewShopOwner()
        {
            Assert.NotNull(ShopOwner.GetShopOwner(meni, menisShop));
        }

        [Test]
        public void testAddManager()
        {
            ShopOwner meniOwner = ShopOwner.GetShopOwner(meni, menisShop);
            User beni = new User("beni","123456");
            //add a new manager with an adding privelage
            List<string> actionsAllowed = new List<string>();
            actionsAllowed.Add("addManager");
            ManagingPrivileges privs = new ManagingPrivileges(actionsAllowed);
            Assert.True(meniOwner.AddManager(beni,privs));
            Assert.NotNull(ShopOwner.GetShopOwner(beni, menisShop));
            ShopOwner beniOwner = ShopOwner.GetShopOwner(beni, menisShop);
            Assert.False(beniOwner.CloseShop());
        }
    }
}
