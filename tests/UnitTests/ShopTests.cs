using DomainLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class ShopTests
    {
        User loggedInUser;
        User notLoggedInUser;
        Product football;

        [SetUp]
        public void Setup()
        {
            User.users = new Dictionary<string, User>();
            loggedInUser = User.Register("meni", "123456");
            notLoggedInUser = User.Register("beni", "123456");
            ShopOwner owner = new ShopOwner(loggedInUser);
            string pass = "123456";
            loggedInUser.Login(loggedInUser.username, pass);
            football = new Product("Football", "Sports");
        }

        [Test, Description("Test Shop Init")]
        public void TestShopInit()
        {
           
        }

        [Test, Description("Test RateShop")]
        public void TestRateProduct()
        {
            Assert.False(football.RateProduct(notLoggedInUser, 5));
            Assert.False(football.RateProduct(loggedInUser, 4));//Didn't buy the product
            Assert.AreEqual(0, football.Rate);

        }
    }
}
