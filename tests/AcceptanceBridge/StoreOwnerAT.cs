using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DomainLayer;
using ATBridge;

namespace Tests
{
    [TestFixture]
    public static class StoreOwnerAT
    {
        [SetUp]
        public static void Setup()
        {
            if (!Tester._initalized)
                AdminAT.InitializationAT();
            if (!Tester._groismanRegistered)
                UserAT.RegisterAT1();
            if (!Tester._groismanConnected)
                UserAT.LoginAT1();
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0)
                RegisteredBuyerAT.CreationOfNewStoreByRegisteredUserAT();
        }
        public static void RunStoreOwnerAT()
        {
            StoreManagmentAT(); //GR 4.1
            AppointmentOfNewOwnerAT(); //GR 4.3
            RemoveOfOwnerAT(); //GR 4.4
            AppointmentOfNewManagerAT(); //GR 4.5  
            RemoveOfManagerAT(); //GR 4.6
        }

        //GR 4.1 - Store's owner can manage store in his store.
        public static void StoreManagmentAT()
        {
            AddingProductAT(); //GR 4.1.1
            RemovingProductAT(); //GR 4.1.2
            EditingProductAT(); //GR 4.1.3
        }

        //GR 4.1.1
        public static void AddingProductAT()
        {
            AddingProductAT1();
            AddingProductAT2();
            AddingProductAT3();
            AddingProductAT4();
        }

        [Test]
        public static void AddingProductAT1()
        {
            Guid productGuid = Tester.PBridge.AddShopProduct(Tester.GroismanGuid, Tester._groismanShop, "Galaxy S9", "Cellphones", 2000, 10);
            if(productGuid.CompareTo(Guid.Empty) == 0)
            {
                Assert.Fail();
            }
            Tester.galaxyGuid = productGuid;
            Assert.Pass();

        }

        [Test]
        public static void AddingProductAT2()
        {
            Guid res = Tester.PBridge.AddShopProduct(Tester.GuestGuid,Tester._groismanShop, "Galaxy S9", "Cellphones", 2000, 10);
            Assert.Zero(res.CompareTo(Guid.Empty));
        }

        [Test]
        public static void AddingProductAT3()
        {
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
            Guid res = Tester.PBridge.AddShopProduct(Tester.GroismanGuid, Tester._groismanShop, "Galaxy S9", "Cellphones", 2000, 10);
            Assert.Zero(res.CompareTo(Guid.Empty));
        }

        [Test]
        public static void AddingProductAT4()
        {
            Guid productGuid = Tester.PBridge.AddShopProduct(Tester.GroismanGuid, Tester._groismanShop, "Galaxy S9", "Cellphones", -2000, 10);
            if (productGuid.CompareTo(Guid.Empty) == 0)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        //GR 4.1.2
        public static void RemovingProductAT()
        {
            RemovingProductAT1();
            RemovingProductAT2();
            RemovingProductAT3();
        }

        [Test]
        public static void RemovingProductAT1()
        {
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
            //After setting the pre-conditions.
            bool result = Tester.PBridge.RemoveShopProduct(Tester.GroismanGuid, Tester.galaxyGuid, Tester._groismanShop);
            Assert.True(result);
            if (result)
            {
                Tester.galaxyGuid = Guid.Empty;
            }
        }

        [Test]
        public static void RemovingProductAT2()
        {
            bool result = Tester.PBridge.RemoveShopProduct(Tester.GuestGuid, Tester.galaxyGuid, Tester._groismanShop);
            Assert.IsFalse(result);
        }

        [Test]
        public static void RemovingProductAT3()
        {
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
            bool result = Tester.PBridge.RemoveShopProduct(Tester.GroismanGuid , Guid.Empty , Tester._groismanShop); //The empty guid doesn't exist in the shop.
            Assert.False(result);
        }



        //GR 4.1.3 
        public static void EditingProductAT()
        {
            EditingProductAT1();
            EditingProductAT2();
            EditingProductAT3();
            EditingProductAT4();
        }

        [Test]
        public static void EditingProductAT1()
        {
            if (Tester._groismanShop.CompareTo(Guid.Empty) == 0 || Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
           bool result = Tester.PBridge.EditShopProduct(Tester.GroismanGuid,Tester._groismanShop, Tester.galaxyGuid, 1500, 20);
           Assert.True(result);
        }

        [Test]
        public static void EditingProductAT2()
        {
            bool result = Tester.PBridge.EditShopProduct(Tester.GuestGuid, Tester._groismanShop, Tester.galaxyGuid, 1500, 20);
            Assert.True(result);
        }

        [Test]
        public static void EditingProductAT3()
        {
            bool result = Tester.PBridge.RemoveShopProduct(Tester.GroismanGuid, Guid.Empty, Tester._groismanShop); //The empty guid doesn't exist in the shop.
            Assert.False(result);
        }

        [Test]
        public static void EditingProductAT4()
        {
            if (Tester.galaxyGuid.CompareTo(Guid.Empty) == 0)
            {
                AddingProductAT1();
            }
            bool result = Tester.PBridge.EditShopProduct(Tester.GroismanGuid, Tester._groismanShop, Tester.galaxyGuid, 1500, -20);
            Assert.False(result);

        }

        //GR 4.3 - Store's owner can appoint new owner to his store.
        public static void AppointmentOfNewOwnerAT()
        {
            AppointmentOfNewOwnerAT1();
            AppointmentOfNewOwnerAT2();
            AppointmentOfNewOwnerAT3();
            //AppointmentOfNewOwnerAT4(); //CAN NOT TEST THAT!
        }

        [Test]
        public static void AppointmentOfNewOwnerAT1()
        {
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                Tester.PBridge.Register(Tester.GuestGuid, "Benhas", "151097");
                Tester.BenGuid = Tester.PBridge.Login(Tester.GuestGuid, "Benhas", "151097");
            }
            bool res = Tester.PBridge.AddShopOwner(Tester.GroismanGuid, Tester._groismanShop, Tester.BenGuid);
            Assert.True(res);
            //TODO: Remove Ben from system
        }

        [Test]
        public static void AppointmentOfNewOwnerAT2()
        {
            bool res = Tester.PBridge.AddShopOwner(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid);
            Assert.False(res);
        }

        [Test]
        public static void AppointmentOfNewOwnerAT3()
        {
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                Tester.PBridge.Register(Tester.GuestGuid, "Benhas", "151097");
                Tester.BenGuid = Tester.PBridge.Login(Tester.GuestGuid, "Benhas", "151097");
            }
            bool res = Tester.PBridge.AddShopOwner(Tester.GuestGuid, Tester._groismanShop, Tester.BenGuid);
            Assert.False(res);
        }

        //CAN NOT TEST THAT!
        /*[Test]
        public static void AppointmentOfNewOwnerAT4()
        {
            Assert.Pass();
        }*/


        //GR 4.4 - Store's owner can remove new owner from his store.
        public static void RemoveOfOwnerAT()
        {
            RemoveOfOwnerAT1();
            RemoveOfOwnerAT2();
            //RemoveOfOwnerAT3(); // can not test it
            RemoveOfOwnerAT4();
        }

        [Test]
        public static void RemoveOfOwnerAT1()
        {
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                AppointmentOfNewOwnerAT1();
            }
            bool res = Tester.PBridge.CascadeRemoveShopOwner(Tester.GroismanGuid, Tester._groismanShop, Tester.BenGuid);
            Assert.True(res);
        }

        [Test]
        public static void RemoveOfOwnerAT2()
        {
            bool res = Tester.PBridge.CascadeRemoveShopOwner(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid);
            Assert.False(res);
        }

        //Can not test it
       /* [Test]
        public static void RemoveOfOwnerAT3()
        {
            Assert.Pass();
        } */

        [Test]
        public static void RemoveOfOwnerAT4()
        {
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                AppointmentOfNewOwnerAT1();
            }
            bool res = Tester.PBridge.CascadeRemoveShopOwner(Tester.GroismanGuid, Tester._groismanShop, Tester.BenGuid);
            Assert.False(res);
        }


        //GR 4.5 - Store's owner can appoint new owner to his store.
        public static void AppointmentOfNewManagerAT()
        {
            //TODO: Add to proxy bridge implemention of adding/appointment of new owner
            AppointmentOfNewManagerAT1();
            AppointmentOfNewManagerAT2();
            AppointmentOfNewManagerAT3();
            //AppointmentOfNewManagerAT4(); // can not test it.
        }

        [Test]
        public static void AppointmentOfNewManagerAT1()
        {
            List<string> privillages = new List<string>();
            privillages.Add("CPA");
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                Tester.PBridge.Register(Tester.GuestGuid, "Benhas", "151097");
                Tester.BenGuid = Tester.PBridge.Login(Tester.GuestGuid, "Benhas", "151097");
            }
            bool res = Tester.PBridge.AddShopManager(Tester.GroismanGuid, Tester._groismanShop, Tester.BenGuid, privillages);
            Assert.True(res);
            //TODO:Remove Ben from manager role.
        }

        [Test]
        public static void AppointmentOfNewManagerAT2()
        {
            List<string> privillages = new List<string>();
            privillages.Add("CPA");
           
            bool res = Tester.PBridge.AddShopManager(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid, privillages);
            Assert.False(res);
        }

        [Test]
        public static void AppointmentOfNewManagerAT3()
        {
            List<string> privillages = new List<string>();
            privillages.Add("CPA");
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                Tester.PBridge.Register(Tester.GuestGuid, "Benhas", "151097");
                Tester.BenGuid = Tester.PBridge.Login(Tester.GuestGuid, "Benhas", "151097");
            }
            bool res = Tester.PBridge.AddShopManager(Tester.GuestGuid, Tester._groismanShop, Tester.BenGuid, privillages);
            Assert.False(res);
        }

        /*[Test]
        public void AppointmentOfNewManagerAT4()
        {
            Assert.Pass();
        }*/


        //GR 4.6 - Store's owner can remove manager from his store.
        public static void RemoveOfManagerAT()
        {
            //TODO:Like GR 4.4
            RemoveOfManagerAT1();
            RemoveOfManagerAT2();
            //RemoveOfManagerAT3(); //Can not test it
            RemoveOfManagerAT4();
        }

        [Test]
        public static void RemoveOfManagerAT1()
        {
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                AppointmentOfNewManagerAT1();
            }
            bool res = Tester.PBridge.RemoveShopManager(Tester.GroismanGuid, Tester._groismanShop, Tester.BenGuid);
            Assert.True(res);
        }

        [Test]
        public static void RemoveOfManagerAT2()
        {
            bool res = Tester.PBridge.RemoveShopManager(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid);
            Assert.False(res);
        }

        /*[Test] //Can not test it
        public static void RemoveOfManagerAT3()
        {
            Assert.Pass();
        }*/

        [Test]
        public static void RemoveOfManagerAT4()
        {
            if (Tester.BenGuid.CompareTo(Guid.Empty) == 0)
            {
                AppointmentOfNewManagerAT1();
            }
            bool res = Tester.PBridge.CascadeRemoveShopOwner(Tester.GuestGuid, Tester._groismanShop, Tester.BenGuid);
            Assert.False(res);
        }


    }
}
