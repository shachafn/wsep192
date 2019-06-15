using System;
using System.Collections.Generic;
using NUnit.Framework;
using ATBridge;
using System.Linq;
using ApplicationCore.Exceptions;

namespace Tests
{
    public static class UserAT
    {
        static Guid _admiNCookie = Guid.NewGuid();

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
        }


        [TearDown]
        public static void TearDown()
        {
            Tester.PBridge.ClearSystem();
        }

        [SetUp]
        public static void SetUp()
        {
            Tester.AdminGuid = Tester.PBridge.Initialize(_admiNCookie, "admin", "000000");
        }


        #region GR 2.2 - User's registration
        [Test]
        public static void RegisterAT1()
        {
            GenerateRandoms(out var cookie, out var username, out var password);
            var result = RegisterUser(cookie, username, password);
            Assert.AreNotEqual(result, Guid.Empty);
        }
        [Test]
        public static void RegisterAT2()
        {
            GenerateRandoms(out var cookie, out var username, out var password);
            RegisterUser(cookie, username, password);
            var second = RegisterUser(cookie, username, password);
            //Same username
            Assert.AreEqual(Guid.Empty, second);
        }
        #endregion

        #region GR 2.3 - login of guest with identifiers.

        [Test]
        public static void LoginAT1()
        {
            GenerateRandoms(out var cookie, out var username, out var password);
            RegisterUser(cookie, username, password);
            var res = LoginUser(cookie, username, password);
            Assert.IsTrue(res);
        }

        [Test]
        public static void LoginAT2()
        {
            GenerateRandoms(out var cookie, out var username, out var password);
            RegisterUser(cookie, username, password);
            var wrongPass = RandomString();
            Assert.Throws<CredentialsMismatchException>( () => LoginUser(cookie, username, wrongPass));
        }
        #endregion

        #region GR 2.5 - search products in the catalog
        [Test]
        public static void SearchProductsAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");
            var galaxyGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            var iphoneGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Iphone 6", "Cellphones", 500, 50);

            var resByName = Tester.PBridge.SearchProduct(cookie, new List<string>() { "Iphone 6" }, "Name");
            Assert.AreEqual(resByName.Count, 1);
            Assert.IsTrue(resByName.Any(t => t.Item1.Guid.Equals(iphoneGuid)));

            resByName = Tester.PBridge.SearchProduct(cookie, new List<string>() { "Galaxy" }, "Name");
            Assert.AreEqual(resByName.Count, 1);
            Assert.IsTrue(resByName.Any(t => t.Item1.Guid.Equals(galaxyGuid)));

            resByName = Tester.PBridge.SearchProduct(cookie, new List<string>() { "OnePlus" }, "Name");
            Assert.AreEqual(resByName.Count, 0);

            var resByCategory = Tester.PBridge.SearchProduct(cookie, new List<string>() { "Cellphones" }, "Category");
            Assert.AreEqual(resByCategory.Count, 2);
            Assert.IsTrue(resByCategory.Any(t => t.Item1.Guid.Equals(iphoneGuid)));
            Assert.IsTrue(resByCategory.Any(t => t.Item1.Guid.Equals(galaxyGuid)));

            resByCategory = Tester.PBridge.SearchProduct(cookie, new List<string>() { "Dishwashers" }, "Category");
            Assert.AreEqual(resByCategory.Count, 0);


            /* Not yet supported from service kayer
            var resByKeywards = Tester.PBridge.SearchProduct(cookie, new List<string>() { "Cellphones" }, "Category");
            CollectionAssert.AreEquivalent(resByName, new List<Guid>() { iphoneGuid, galaxyGuid });
            resByCategory = Tester.PBridge.SearchProduct(cookie, new List<string>() { "Dishwashers" }, "Category");
            CollectionAssert.AreEquivalent(resByName, new List<Guid>());
            */
        }
        [Test]
        public static void SearchProductsAT2()
        {

            if (!Tester._groismanRegistered)
                RegisterAT1();
            if (!Tester._groismanConnected)
                LoginAT1();
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0)
                RegisteredBuyerAT.OpenStoreAT1(); //the shop is empty and no product is available.
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
                StoreOwnerAT.AddingProductAT1(); //now Groisman's shop has 10 Galaxys
            //TODO_REMOVE_COMMENT Assert.IsNotNull(Tester.PBridge.SearchProduct(Tester.GuestGuid, Tester._groismanShop, "IPhone 6"));
        }
        #endregion

        public static bool LoginUser(Guid cookie, string username, string password)
        {
            return Tester.PBridge.Login(cookie, username, password);
        }

        public static Guid RegisterUser(Guid cookie, string username, string password)
        {
            return Tester.PBridge.Register(cookie, username, password);
        }

        public static void GenerateRandoms(out Guid cookie, out string username, out string password)
        {
            cookie = Guid.NewGuid();
            username = RandomString();
            password = RandomString();
        }
        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
