using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
using DomainLayer;
using DomainLayer.Exceptions;

namespace Tests
{
    public static class UserAT
    {
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
            Tester.AdminGuid = Tester.PBridge.Initialize(Tester.GuestGuid, "admin", "000000");
        }


        //GR 2.2 - User's registration
        public static void RegisterAT()
        {
            RegisterAT1();
            RegisterAT2();
        }

        [Test]
        public static void RegisterAT1()
        {
            Tester.GroismanGuid = Tester.PBridge.Register(Tester.GuestGuid, "groisman", "150298");
            Assert.AreNotEqual(Tester.GroismanGuid, Guid.Empty);
        }
        [Test]
        public static void RegisterAT2()
        {
            RegisterAT1();
            //Same username
            Assert.AreEqual(Tester.PBridge.Register(Tester.GuestGuid, "groisman", "1111"), Guid.Empty);
        }
        //GR 2.3-login of guest with identifiers.
        public static void LoginAT()
        {
            LoginAT1();
            LoginAT2();
        }

        [Test]
        public static void LoginAT1()
        {
            string exist_username = "groisman";
            string exist_password = "150298";
            if (!Tester._groismanRegistered)
                RegisterAT1();
            if (!Tester._groismanConnected)
            {
                Tester._groismanConnected = true;
                Guid groisman = Tester.PBridge.Login(Tester.GuestGuid, exist_username, exist_password);
                Assert.NotZero(groisman.CompareTo(Guid.Empty));
                Tester.GroismanGuid = groisman;
            }
        }

        [Test]
        public static void LoginAT2()
        {
            string username = "idoGroiser";
            string password = "090902";
            Assert.Throws<CredentialsMismatchException>(
                () =>
                Tester.PBridge.Login(Tester.GuestGuid, username, password));
        }

        //GR 2.5 - search products in the catalog
        public static void SearchProductsAT()
        {
            SearchProductsAT1();
            SearchProductsAT2();
        }
        [Test]
        public static void SearchProductsAT1()
        {
            if (!Tester._groismanRegistered)
                RegisterAT1();
            if (!Tester._groismanConnected)
                LoginAT1();
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0)
                RegisteredBuyerAT.OpenStoreAT1(); //the shop is empty and no product is available.
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
                StoreOwnerAT.AddingProductAT1(); //now Groisman's shop has 10 Galaxys
            Assert.IsNotNull(Tester.PBridge.SearchProduct(Tester.GuestGuid, Tester._groismanShop, "Galaxy S9"));
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
            Assert.IsNotNull(Tester.PBridge.SearchProduct(Tester.GuestGuid, Tester._groismanShop, "IPhone 6"));
        }

        //GR 2.6 - Saving/Adding products in user's cart
        public static void SavingProductsInCartAT()
        {
            SavingProductsInCartAT1();
            SavingProductsInCartAT2();
        }
        [Test]
        public static void SavingProductsInCartAT1()
        {
            Tester.PBridge.ChangeUserState(Tester.GuestGuid, "BuyerUserState"); //Move to buyer state
            bool res = Tester.PBridge.AddProductToShoppingCart(Tester.GuestGuid, Tester.galaxyGuid, Tester._groismanShop, 1);
            Assert.True(res);
        }
        [Test]
        public static void SavingProductsInCartAT2()
        {
            Tester.PBridge.ChangeUserState(Tester.GuestGuid, "BuyerUserState"); //Move to buyer state
            bool res = Tester.PBridge.AddProductToShoppingCart(Tester.GuestGuid, Guid.Empty , Tester._groismanShop, 1); //Invalid product.
            Assert.False(res);
        }

        //GR 2.7- watching and editing of cart
        public static void WatchingAndEditingOfCartAT()
        {
            WatchingAndEditingOfCartAT1();
            WatchingAndEditingOfCartAT2();
            WatchingAndEditingOfCartAT3();
        }
        [Test]
        public static void WatchingAndEditingOfCartAT1()
        {
            if (!Tester._groismanRegistered)
                RegisterAT1();
            if (!Tester._groismanConnected)
                LoginAT1();
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0)
                RegisteredBuyerAT.OpenStoreAT1(); //the shop is empty and no product is available.
            Tester.PBridge.ChangeUserState(Tester.GuestGuid, "BuyerUserState"); //Move to buyer state
            Assert.IsNull(Tester.PBridge.GetAllProductsInCart(Tester.GuestGuid, Tester._groismanShop));
        }
        [Test]
        public static void WatchingAndEditingOfCartAT2()
        {
            if (!Tester._groismanRegistered)
                RegisterAT1();
            if (!Tester._groismanConnected)
                LoginAT1();
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0)
                RegisteredBuyerAT.OpenStoreAT1(); //the shop is empty and no product is available.
            if(Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
                StoreOwnerAT.AddingProductAT1(); //now Groisman's shop has 10 Galaxys

            Tester.PBridge.ChangeUserState(Tester.GuestGuid, "BuyerUserState"); //Move to buyer state
            SavingProductsInCartAT1(); //Add 1 galaxy
            ICollection<Guid> expected = new LinkedList<Guid>();
            expected.Add(Tester.galaxyGuid); //Assumption : The list will hold product guid and not shopProduct guid.
            Assert.AreEqual(expected,Tester.PBridge.GetAllProductsInCart(Tester.GuestGuid, Tester._groismanShop));
        }
        [Test]
        public static void WatchingAndEditingOfCartAT3()
        {
            if (!Tester._groismanRegistered)
                RegisterAT1();
            if (!Tester._groismanConnected)
                LoginAT1();
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0)
                RegisteredBuyerAT.OpenStoreAT1(); //the shop is empty and no product is available.
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
                StoreOwnerAT.AddingProductAT1(); //now Groisman's shop has 10 Galaxys
            SavingProductsInCartAT1(); //Add product to cart.
            Tester.PBridge.ChangeUserState(Tester.GuestGuid, "BuyerUserState"); //Move to buyer state
            bool res = Tester.PBridge.RemoveProductFromCart(Tester.GuestGuid, Tester._groismanShop, Tester.galaxyGuid);
            Assert.True(res);
        }

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

        public static void RunUserAT()
        {
            RegisterAT();
            LoginAT(); //GR 2.3
            SearchProductsAT(); //GR 2.5
            SavingProductsInCartAT();//GR 2.6
            WatchingAndEditingOfCartAT(); // GR 2.7
            //PurchaseAT(); //GR 2.8
        }
    }
}
