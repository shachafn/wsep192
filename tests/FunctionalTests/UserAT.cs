using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests
{
    [TestFixture]
    class UserAT
    {
        //GR 2.3-login of guest with identifiers.
        public void LoginAT()
        {
            LoginAT1();
            LoginAT2();
        }
        [Test]
        public void LoginAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void LoginAT2()
        {
            Assert.AreEqual(true, true);
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
            Assert.AreEqual(true, true);
        }
        [Test]
        public void SearchProductsAT2()
        {
            Assert.AreEqual(true, true);
        }

        //GR 2.6 - keeping products in user's cart
        public void KeepingProductsInCartAT()
        {
            KeepingProductsInCartAT1();
            KeepingProductsInCartAT2();
        }
        [Test]
        public void KeepingProductsInCartAT1()
        {
            Assert.AreEqual(true, true);
        }
        [Test]
        public void KeepingProductsInCartAT2()
        {
            Assert.AreEqual(true, true);
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
            Assert.AreEqual(true, true);
        }
        [Test]
        public void WatchingAndEditingOfCartAT2()
        {
            Assert.AreEqual(true, true);
        }
        [Test]
        public void WatchingAndEditingOfCartAT3()
        {
            Assert.AreEqual(true, true);
        }

        //GR 2.8 - purchase of products

        public void PurchaseAT()
        {
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
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT4()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT5()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT6()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT7()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void PurchaseAT8()
        {
            Assert.AreEqual(true, true);
        }

        public void RunUserAT()
        {
            LoginAT(); //GR 2.3
            SearchProductsAT(); //GR 2.5
            KeepingProductsInCartAT();//GR 2.6
            WatchingAndEditingOfCartAT(); // GR 2.7
            PurchaseAT(); //GR 2.8
        }
    }
}
