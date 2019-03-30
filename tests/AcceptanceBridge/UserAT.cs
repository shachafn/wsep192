using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
using DomainLayer;

namespace Tests
{
    [TestFixture]
    class UserAT
    {
        private ProxyBridge _proxy;
        private User _aUser;
        private User _gusetUser;
        private Product _IPhone;
        private Product _Carrot;
        private UserInfo _info;
        private ShoppingCart _cart;
        private ShopOwner _owner;
        private Shop _shop;
        [SetUp]
        public void Setup()
        {
            _proxy = new ProxyBridge();
            _proxy.SetRealBridge(new BridgeImpl());
             _proxy.Register("groisman", "150298");
            _aUser = new User("groisman", "150298");
            _gusetUser = new User();
            _IPhone = new Product("IPhone", "Cellphones");
            _Carrot = new Product("Carrot", "Vegtables");
            _owner = new ShopOwner(_aUser, _shop, true);
            _shop = new Shop(_owner);
            _cart = new ShoppingCart(_shop); //I saw liron's branch.
            //_cart.addProduct(_IPhone);
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
            Assert.AreEqual(true,_proxy.Login(exist_username, exist_password));
            _proxy.Logout();
        }

        [Test]
        public void LoginAT2()
        {
            Setup();
            string username = "notExist";
            string password = "notExist";
            Assert.AreEqual(false, _proxy.Login(username, password));
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
            //Setup();
            List<Product> acceptedList = new List<Product>();
            acceptedList.Add(_IPhone);
            Assert.AreEqual(acceptedList, _proxy.SearchProduct("IPhone"));
            //TODO: Change null to some Product when I'll know how to add product to some store.
        }
        [Test]
        public void SearchProductsAT2()
        {
            //Setup();
            List<Product> acceptedList = new List<Product>();
            Assert.AreEqual(acceptedList, _proxy.SearchProduct("Galaxy S9"));
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
            Setup(); //Create a username "groismanGuest" with pswd "150298"
            //TODO: 1.Save user changes should be public and return some info.
            //TODO: 2.Change Pass to some missing method operation in ProxyBridge

            Assert.Pass();
        }
        [Test]
        public void KeepingProductsInCartAT2()
        {
            //TODO: Add something to cart
            //_aUser.saveUserChanges(); 
            //TODO: 1.Save user changes should be public and return some info.
            //TODO: 2.Change Pass to some missing method operation in ProxyBridge
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
            Setup();
            ShoppingCart emptyCart = new ShoppingCart();
            //List<Product> em = new List<Product>();
            Assert.AreEqual(emptyCart, _proxy.GetAllProducts(emptyCart)); //Assume proxy gets the cart.
        }
        [Test]
        public void WatchingAndEditingOfCartAT2()
        {
            ShoppingCart oneItemCart = new ShoppingCart();
            Assert.AreEqual(true,_proxy.AddProduct(_IPhone,oneItemCart)); //Assume proxy gets the cart.
        }
        [Test]
        public void WatchingAndEditingOfCartAT3()
        {
            //_cart = cart with one IPhone.
            Assert.AreEqual(true, _proxy.RemoveProduct(_IPhone,_cart)); //Assume it gets product and shopping cart.
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
            //TODO: Complete when I'll know how to purchase a product.
            Setup();

            Assert.Pass();
        }

        [Test]
        public void PurchaseAT2()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public void PurchaseAT3()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public void PurchaseAT4()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public void PurchaseAT5()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public void PurchaseAT6()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public void PurchaseAT7()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
        }

        [Test]
        public void PurchaseAT8()
        {
            //TODO: Complete when I'll know how to purchase a product.
            Assert.Pass();
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
