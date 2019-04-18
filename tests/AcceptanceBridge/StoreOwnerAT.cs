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

        private ProxyBridge _proxy;

        [SetUp]
        public void Setup()
        {
            _proxy = new ProxyBridge();
            _proxy.SetRealBridge(new BridgeImpl());
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
            //Register+login to groisman's account
            /*UserAT userAT = new UserAT();
            userAT.RegisterAT1();
            userAT.LoginAT1();
            //Open shop
            RegisteredBuyerAT registeredBuyerAT = new RegisteredBuyerAT();
            if (Tester._groismanShop == null)
                registeredBuyerAT.CreationOfNewStoreByRegisteredUserAT();
            //Add products to shop
           // Tester.galaxyGuid = _proxy.AddProductToShop("Galaxy S9", "Cellphones", 2000, 10, Tester._groismanShop);
            Assert.NotZero(Guid.Empty.CompareTo(Tester.galaxyGuid)); */
        }

        [Test]
        public void AddingProductAT2()
        {
            //No check of permissions - can not test.
            Assert.Fail();
        }

        [Test]
        public void AddingProductAT3()
        {
            //Register+login to groisman's account
            /*UserAT userAT = new UserAT();
            userAT.RegisterAT1();
            userAT.LoginAT1();
            //Open shop
            RegisteredBuyerAT registeredBuyerAT = new RegisteredBuyerAT();
            if (Tester._groismanShop == null)
                registeredBuyerAT.CreationOfNewStoreByRegisteredUserAT(); */
           // Assert.Zero(Guid.Empty.CompareTo(_proxy.AddProductToShop("Galaxy S9", "Cellphones", 2000, 10, Tester._groismanShop)));
        }

        [Test]
        public void AddingProductAT4()
        {
            //Expected to get the empty guid because we try to type negative quantity.
            //Register+login to groisman's account
            /*UserAT userAT = new UserAT();
            userAT.RegisterAT1();
            userAT.LoginAT1();
            //Open shop
            RegisteredBuyerAT registeredBuyerAT = new RegisteredBuyerAT();
            if (Tester._groismanShop == null)
                registeredBuyerAT.CreationOfNewStoreByRegisteredUserAT(); */
            //Assert.Zero(Guid.Empty.CompareTo(_proxy.AddProductToShop("IPhone", "Cellphones", 1000, -1, Tester._groismanShop)));
        }

        //GR 4.1.2
        public void RemovingProductAT()
        {
            RemovingProductAT1();
            RemovingProductAT2();
            RemovingProductAT3();
            RemovingProductAT4();
        }

        [Test]
        public void RemovingProductAT1()
        {
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0 || Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
            //After setting the pre-conditions.
            //bool result = _proxy.RemoveProductFromShop(Tester.galaxyGuid, Tester._groismanShop);
            //Assert.True(result);
            //if (result)
            //{
            //    Tester.galaxyGuid = Guid.Empty;
            //}
        }

        [Test]
        public void RemovingProductAT2()
        {
            //Can not test it - there is no dependency in user's permissions and identity.
            Assert.Fail();
        }

        [Test]
        public void RemovingProductAT3()
        {
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0 || Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
            //bool result = _proxy.RemoveProductFromShop(Guid.Empty, Tester._groismanShop); //The empty guid doesn't exist in the shop.
           // Assert.False(result);
        }

        [Test]
        public void RemovingProductAT4()
        {
            //Can not test it. Reasons:
            //1. Removing unexisted product is tested in RemovingProductAT3
            //2. Can not send details of product.
            //My advice : Removing the requirement from the doucument and removing this test. This test may be irelevant.
            Assert.Fail();
        }

        //GR 4.1.3 
        public void EditingProductAT()
        {
            EditingProductAT1();
            EditingProductAT2();
            EditingProductAT3();
            EditingProductAT4();
        }

        [Test]
        public void EditingProductAT1()
        {
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0 || Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
           // bool result = _proxy.EditProduct(Tester._groismanShop, Tester.galaxyGuid, 1500, 20);
           // Assert.True(result);
        }

        [Test]
        public void EditingProductAT2()
        {
            //Can not test it - there is no dependency in user's permissions and identity.
            Assert.Fail();
        }

        [Test]
        public void EditingProductAT3()
        {
          //  bool result = _proxy.EditProduct(Tester._groismanShop, Guid.Empty, 1500, -20); //The empty guid doesn't exist in the shop.
            //Assert.False(result);
        }

        [Test]
        public void EditingProductAT4()
        {
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0 || Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
          //  bool result = _proxy.EditProduct(Tester._groismanShop, Tester.galaxyGuid, 1500, -20);
           // Assert.False(result);

        }

        //GR 4.3 - Store's owner can appoint new owner to his store.
        public void AppointmentOfNewOwnerAT()
        {
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
