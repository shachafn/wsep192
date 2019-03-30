using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DomainLayer;
using ATBridge;

namespace Tests
{
    [TestFixture]
    class StoreOwnerAT
    {
        private User _user;
        private User _unRegisteredUser;
        private ShopOwner _owner;
        private ShopOwner _fakeOwner;
        private Shop _shop;
        private ProxyBridge _proxy;
        private Product _productToAdd;
        private ShopProduct _product; 
        [SetUp]
        public void Setup()
        {
            _proxy = new ProxyBridge();
            _proxy.SetRealBridge(new BridgeImpl());
            _user = _proxy.Register("groisman", "150298");
            _unRegisteredUser = new User("tommarz", "311297");
            _proxy.Login("groisman", "king98");
            _owner = new ShopOwner(_user,_shop,true);
            _fakeOwner = new ShopOwner(_fakeOwner)
            _shop = new Shop(_owner);
            _productToAdd = new Product("IPhone", "Cellphones");
            _product = new ShopProduct(_productToAdd, 1000, 1);
        }
        public void RunStoreOwnerAT()
        {
            StoreManagmentAT(); //GR 4.1
            AppointmentOfNewOwnerAT(); //GR 4.3
            RemoveOfOwnerAT(); //GR 4.4
            AppointmentOfNewManagerAT(); //GR 4.5  
            RemoveOfManagerAT(); //GR 4.6
        }

        //GR 4.1 - Store's owner can manage store in his store.
        public void StoreManagmentAT()
        {
            AddingProductAT(); //GR 4.1.1
            RemovingProductAT(); //GR 4.1.2
            EditingProductAT(); //GR 4.1.3
        }

        //GR 4.1.1
        public void AddingProductAT()
        {
            AddingProductAT1();
            AddingProductAT2();
            AddingProductAT3();
            AddingProductAT4();
        }

        [Test]
        public void AddingProductAT1()
        {
            Assert.AreEqual(true, _proxy.AddProduct(_product));
        }

        [Test]
        public void AddingProductAT2()
        {
            
            Assert.Pass();
        }

        [Test]
        public void AddingProductAT3()
        {
            //TODO: Make class of ShopProduct in order to be able to test it.
            Assert.Pass();
        }

        [Test]
        public void AddingProductAT4()
        {
            //TODO: Make class of ShopProduct in order to be able to test it.
            Assert.Pass();
        }

        //GR 4.1.2
        public void RemovingProductAT()
        {
            //TDDO: Fix add product in proxy in order to make this test possible.
            RemovingProductAT1();
            RemovingProductAT2();
            RemovingProductAT3();
            RemovingProductAT4();
        }

        [Test]
        public void RemovingProductAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void RemovingProductAT2()
        {
            Assert.Pass();
        }

        [Test]
        public void RemovingProductAT3()
        {
            Assert.Pass();
        }

        [Test]
        public void RemovingProductAT4()
        {
            Assert.Pass();
        }

        //GR 4.1.3 
        public void EditingProductAT()
        {
            //TDDO: Fix add product in proxy in order to make this test possible. 
            EditingProductAT1();
            EditingProductAT2();
            EditingProductAT3();
        }

        [Test]
        public void EditingProductAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void EditingProductAT2()
        {
            Assert.Pass();
        }

        [Test]
        public void EditingProductAT3()
        {
            Assert.Pass();
        }

        //GR 4.3 - Store's owner can appoint new owner to his store.
        public void AppointmentOfNewOwnerAT()
        {
            //TODO: Add to proxy bridge implemention of adding/appointment of new owner
            AppointmentOfNewOwnerAT1();
            AppointmentOfNewOwnerAT2();
            AppointmentOfNewOwnerAT3();
            AppointmentOfNewOwnerAT4();
        }

        [Test]
        public void AppointmentOfNewOwnerAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void AppointmentOfNewOwnerAT2()
        {
            Assert.Pass();
        }

        [Test]
        public void AppointmentOfNewOwnerAT3()
        {
            Assert.Pass();
        }

        [Test]
        public void AppointmentOfNewOwnerAT4()
        {
            Assert.Pass();
        }


        //GR 4.4 - Store's owner can remove new owner from his store.
        public void RemoveOfOwnerAT()
        {
            //TODO: Add to proxy bridge implemention of owner's removing
            RemoveOfOwnerAT1();
            RemoveOfOwnerAT2();
            RemoveOfOwnerAT3();
            RemoveOfOwnerAT4();
        }

        [Test]
        public void RemoveOfOwnerAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfOwnerAT2()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfOwnerAT3()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfOwnerAT4()
        {
            Assert.Pass();
        }


        //GR 4.5 - Store's owner can appoint new owner to his store.
        public void AppointmentOfNewManagerAT()
        {
            //TODO: Add to proxy bridge implemention of adding/appointment of new owner
            AppointmentOfNewManagerAT1();
            AppointmentOfNewManagerAT2();
            AppointmentOfNewManagerAT3();
            AppointmentOfNewManagerAT4();
        }

        [Test]
        public void AppointmentOfNewManagerAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void AppointmentOfNewManagerAT2()
        {
            Assert.Pass();
        }

        [Test]
        public void AppointmentOfNewManagerAT3()
        {
            Assert.Pass();
        }

        [Test]
        public void AppointmentOfNewManagerAT4()
        {
            Assert.Pass();
        }


        //GR 4.6 - Store's owner can remove manager from his store.
        public void RemoveOfManagerAT()
        {
            //TODO:Like GR 4.4
            RemoveOfManagerAT1();
            RemoveOfManagerAT2();
            RemoveOfManagerAT3();
            RemoveOfManagerAT4();
        }

        [Test]
        public void RemoveOfManagerAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfManagerAT2()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfManagerAT3()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfManagerAT4()
        {
            Assert.Pass();
        }


    }
}
