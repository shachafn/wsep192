using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ShopTests
    {
        Shop shop;
        Shop shop1;

        [SetUp]
        public void Setup()
        {
            shop = new Shop();
            shop1 = new Shop();
        }

        [Test, Description("Test Shop Init")]
        public void TestShopInit()
        {
            Assert.NotNull(shop);
            Assert.AreEqual(0, shop.Rate);
            Assert.AreEqual(0, shop.State);
            Assert.AreNotEqual(shop.ShopGuid, shop1.ShopGuid);
        }
        [Test, Description("Test Shop States")]
        public void TestShopStates()
        {
            shop.Close();
            Assert.AreEqual(1, shop.State);
            shop.Open();
            Assert.AreEqual(0, shop.State);
            shop.Adminclose();
            Assert.AreEqual(2, shop.State);
            shop.Open();
            Assert.AreEqual(2, shop.State);
            shop.Close();
            Assert.AreEqual(2, shop.State);
        }
    }
}
