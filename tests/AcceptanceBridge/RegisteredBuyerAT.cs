using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DomainLayer;
using ATBridge;
using DomainLayer.Exceptions;

namespace Tests
{
    [TestFixture]
    public static class RegisteredBuyerAT
    {

        

        [SetUp]
        public static void SetUp()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
            if (!Tester._initalized)
                AdminAT.InitializationAT();
            if (!Tester._groismanRegistered)
                UserAT.RegisterAT1();
            if(!Tester._groismanConnected)
                UserAT.LoginAT1();
            //TODO: MAYBE I will need to remove groisman's shop
        }

        //GR 3.1 - Registered user can commit logout.
        public static void LogoutAT()
        {
            LogoutAT1();
            LogoutAT2();
        }

        [Test]
        public static void LogoutAT1()
        {
            Assert.IsTrue(Tester.PBridge.Logout(Tester.GroismanGuid));
            Tester._groismanConnected = false;
        }

        [Test]
        public static void LogoutAT2()
        {
            try
            {
                Tester.PBridge.Logout(Tester.GuestGuid);
                Assert.Fail();
            }
            catch (IllegalOperationException)
            {
                Assert.Pass();
            }
        }

        //GR 3.2 - Registered user can open new store.
        public static void OpenStoreAT()
        {
            OpenStoreAT1();
            OpenStoreAT2();
        }
       [Test]
        public static void OpenStoreAT1()
        {
            Tester._groismanShop = Tester.PBridge.OpenShop(Tester.GroismanGuid);
            Assert.NotZero(Tester._groismanShop.CompareTo(Guid.Empty));
        }

        [Test]
        public static void OpenStoreAT2()
        {
            try
            {
                Tester.PBridge.OpenShop(Tester.GuestGuid);
                Assert.Fail();
            }
            catch (UserNotFoundException)
            {
                Assert.Pass();
            }
        }

        public static void RunRegisteredUserAT()
        {
            OpenStoreAT();
            LogoutAT();

        }


    }
}
