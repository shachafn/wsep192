using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DomainLayer;
using ATBridge;

namespace Tests
{
    [TestFixture]
    public static class RegisteredBuyerAT
    {

        //GR 3.1 - Registered user can commit logout.

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

        [Test]
        public static void LogoutAT()
        {
            Assert.IsTrue(Tester.PBridge.Logout(Tester.GroismanGuid));
            Tester._groismanConnected = false;
        }

        //GR 3.2 - Registered user can open new store.
        [Test]
        public static void CreationOfNewStoreByRegisteredUserAT()
        {
            Tester._groismanShop = Tester.PBridge.OpenShop(Tester.GroismanGuid);
            Assert.NotZero(Tester._groismanShop.CompareTo(Guid.Empty));
        }

        public static void RunRegisteredUserAT()
        {
            CreationOfNewStoreByRegisteredUserAT();
            LogoutAT();

        }
    }
}
