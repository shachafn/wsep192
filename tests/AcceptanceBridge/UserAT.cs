using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
using DomainLayer;

namespace Tests
{
    //[TestFixture]
    //private Guid g = new Guid();
    public static class UserAT
    {

        [SetUp]
        public static void Setup()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
            //AdminAT.InitializationAT(); //TODO: Add that
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

            Assert.IsTrue(Tester.PBridge.Register(Tester.GuestGuid,"groisman", "150298"));
        }
        [Test]
        public static void RegisterAT2()
        {
            Assert.IsFalse(Tester.PBridge.Register(Tester.GuestGuid,"groisman", "1111")); //invalid password. 
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
            RegisterAT1();
            if (!Tester._groismanConnected)
            {
                Tester._groismanConnected = true;
                Guid groisman = Tester.PBridge.Login(Tester.GuestGuid, exist_username, exist_password);
                Assert.NotZero(groisman.CompareTo(Guid.Empty));
            }
            Assert.Pass();
        }

        [Test]
        public static void LoginAT2()
        {
            string username = "idoGroiser";
            string password = "090902";
            Guid notFoundUser = Tester.PBridge.Login(Tester.GuestGuid, username, password);
            Assert.Zero(notFoundUser.CompareTo(Guid.Empty));
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
            Assert.Fail();
        }
        [Test]
        public static void SearchProductsAT2()
        {
            Assert.Fail();
        }

        //GR 2.6 - Saving products in user's cart
        public static void SavingProductsInCartAT()
        {
            //TODO
            SavingProductsInCartAT1();
            SavingProductsInCartAT2();
        }
        [Test]
        public static void SavingProductsInCartAT1()
        {
            Assert.Pass();
        }
        [Test]
        public static void SavingProductsInCartAT2()
        {
            Assert.Pass();
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

        }
        [Test]
        public static void WatchingAndEditingOfCartAT2()
        {

        }
        [Test]
        public static void WatchingAndEditingOfCartAT3()
        {

        }

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
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT2()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT3()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT4()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT5()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT6()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT7()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public static void PurchaseAT8()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        public static void RunUserAT()
        {
            RegisterAT();
            LoginAT(); //GR 2.3
            SearchProductsAT(); //GR 2.5
            SavingProductsInCartAT();//GR 2.6
            WatchingAndEditingOfCartAT(); // GR 2.7
            PurchaseAT(); //GR 2.8
        }
    }
}
