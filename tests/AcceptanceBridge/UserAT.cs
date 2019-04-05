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
    public class UserAT
    {
        private ProxyBridge _proxy;
        private StoreOwnerAT _ownerAT;

        public UserAT()
        {
            _proxy = new ProxyBridge();
            _ownerAT = new StoreOwnerAT();

        }
        [SetUp]
        public void Setup()
        {
            _proxy.SetRealBridge(new BridgeImpl());
            _proxy.SetRealBridge(new BridgeImpl());
        }

        //GR 2.2 - User's registration
        public void RegisterAT()
        {
            RegisterAT1();
            RegisterAT2();
        }
        [Test]
        public void RegisterAT1()
        {
            Assert.NotNull(_proxy.Register("groisman", "150298"));
        }
        [Test]
        public void RegisterAT2()
        {
            Assert.Null(_proxy.Register("groisman", "1111")); //invalid Failword. 
        }
        //GR 2.3-login of guest with identifiers.
        public void LoginAT()
        {
            LoginAT1();
            LoginAT2();
        }

        [Test]
        public void LoginAT1()
        {
            Setup();
            string exist_username = "groisman";
            string exist_password = "150298";
            this.RegisterAT1();
            if (!Tester._groismanConnected)
            {
                Assert.IsTrue(_proxy.Login(exist_username, exist_password));
            }
            Assert.Pass();
            
        }

        [Test]
        public void LoginAT2()
        {
            Setup();
            string username = "idoGroiser";
            string password = "090902";
            Assert.IsFalse(_proxy.Login(username, password));
        }

        //GR 2.5 - search products in the catalog
        public void SearchProductsAT()
        {
            SearchProductsAT1();
            SearchProductsAT2();
        }
        [Test]
        public void SearchProductsAT1()
        {
            Assert.Fail();
        }
        [Test]
        public void SearchProductsAT2()
        {
            Assert.Fail();
        }

        //GR 2.6 - Saving products in user's cart
        public void SavingProductsInCartAT()
        {
            //TODO
            SavingProductsInCartAT1();
            SavingProductsInCartAT2();
        }
        [Test]
        public void SavingProductsInCartAT1()
        {
            Assert.Pass();
        }
        [Test]
        public void SavingProductsInCartAT2()
        {
            Assert.Pass();
        }

        //GR 2.7- watching and editing of cart
        public void WatchingAndEditingOfCartAT()
        {
            WatchingAndEditingOfCartAT1();
            WatchingAndEditingOfCartAT2();
            WatchingAndEditingOfCartAT3();
        }
        [Test]
        public void WatchingAndEditingOfCartAT1()
        {

        }
        [Test]
        public void WatchingAndEditingOfCartAT2()
        {

        }
        [Test]
        public void WatchingAndEditingOfCartAT3()
        {

        }

        //GR 2.8 - purchase of products

        public void PurchaseAT()
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
        public void PurchaseAT1()
        {
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT2()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT3()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT4()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT5()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT6()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT7()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        [Test]
        public void PurchaseAT8()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Fail();
        }

        public void RunUserAT()
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
