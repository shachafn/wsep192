using System;
using System.Collections.Generic;
using NUnit.Framework;
using ATBridge;
using ApplicationCore.Exceptions;
using System.Linq;
using ApplicationCore.Entitites;
using Newtonsoft.Json;

namespace Tests
{
    [TestFixture]
    public static class RegisteredBuyerAT
    {
        static Guid _adminCookie = Guid.NewGuid();
        static string _adminUsername = "admin";
        static string _adminPassword = "000000";

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
            Tester.AdminGuid = Tester.PBridge.Initialize(_adminCookie, _adminUsername, _adminPassword);
        }

        #region GR 3.1 - Registered user can commit logout.
        [Test]
        public static void LogoutAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var res = Tester.PBridge.Logout(cookie);
            Assert.IsTrue(res);
        }

        [Test]
        public static void LogoutAT2()
        {
            Assert.Throws<IllegalOperationException>(() => Tester.PBridge.Logout(Guid.NewGuid()));
        }
        #endregion

        #region GR 3.2 - Registered user can open new store.
        [Test]
        public static void OpenStoreAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var res = Tester.PBridge.OpenShop(cookie, "Name");
            Assert.AreNotEqual(res, Guid.Empty);
        }

        [Test]
        public static void OpenStoreAT2()
        {
           Assert.Throws<UserNotFoundException>( () => Tester.PBridge.OpenShop(Guid.NewGuid(), "Name"));
        }
        #endregion

        #region GR 2.6 - Saving/Adding products in user's cart

        [Test]
        public static void SavingProductsInCartAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");
            var productGuid = StoreOwnerAT.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);

            var res = Tester.PBridge.AddProductToCart(cookie, shopGuid, productGuid, 1);
            Assert.IsTrue(res);
        }
        [Test]
        public static void SavingProductsInCartAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");
            var productGuid = StoreOwnerAT.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);

            var invalidProductGuid = Guid.Empty;
            Assert.Throws<ProductNotFoundException>(
                () => Tester.PBridge.AddProductToCart(cookie, shopGuid, invalidProductGuid, 1));
        }
        #endregion

        #region  GR 2.7- watching and editing of cart

        [Test]
        public static void WatchingAndEditingOfCartAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");

            var res = Tester.PBridge.GetAllProductsInCart(cookie, shopGuid);
            CollectionAssert.IsEmpty(res);
        }
        [Test]
        public static void WatchingAndEditingOfCartAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "name");
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            var shopProduct = new ShopProduct(new Product("Galaxy S9", "Cellphones"), 2000, 10);
            shopProduct.Guid = productGuid;
            Tester.PBridge.AddProductToCart(cookie, shopGuid, productGuid, 10);

            var res = Tester.PBridge.GetAllProductsInCart(cookie, shopGuid);
            var serResult = JsonConvert.SerializeObject(res);
            var expected = JsonConvert.SerializeObject(new List<ShopProduct>() { shopProduct });
            Assert.AreEqual(expected, serResult);
        }
        [Test]
        public static void WatchingAndEditingOfCartAT3()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);

            var shopGuid = Tester.PBridge.OpenShop(cookie,"Name");
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);

            Tester.PBridge.AddProductToCart(cookie, shopGuid, productGuid, 10);
            Tester.PBridge.RemoveProductFromCart(cookie, shopGuid, productGuid);
            var res = Tester.PBridge.GetAllProductsInCart(cookie, shopGuid);
            CollectionAssert.IsEmpty(res);
        }
        #endregion

        /*
        //GR 2.8 - purchase of products

        public static void PurchaseAT()
        {
            //TODO
            PurchaseAT1();
            PurchaseAT2();
            PurchaseAT3();
            PurchaseAT4();
            PurchaseAT5();
            PurchaseAT6();
            PurchaseAT7();
            PurchaseAT8();
        }

        [Test]
        public static void PurchaseAT1()
        {
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT2()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT3()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT4()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT5()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT6()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT7()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public static void PurchaseAT8()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        } */
    }
}
