using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;

namespace Tests
{
    public static class AdminAT
    {
        [SetUp]
        public static void Setup()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
        }
        //GR 1 - Initialization of the system
        [Test]
        public static void InitializationAT()
        {
            Assert.IsTrue(Tester.PBridge.Initialize(Tester.GuestGuid, "admin", "000000"));
            Tester.AdminGuid = Tester.PBridge.Login(Tester.GuestGuid, "admin", "000000"); //Created in init
        }
        //GR 6.2 - Removing of registered user

        public static void RemoveOfRegisteredUserAT()
        {
            RemoveOfRegisteredUserAT1();
            RemoveOfRegisteredUserAT2();
        }

        [Test]
        public static void RemoveOfRegisteredUserAT1()
        {
            if (!Tester._initalized) {
                InitializationAT();
                Tester._initalized = true;
            }
            Tester.PBridge.Login(Tester.GuestGuid, "admin", "000000");
            if (!Tester._groismanRegistered) {
                UserAT.RegisterAT1();
                Tester._groismanRegistered = true;
            }
            bool res = Tester.PBridge.RemoveUser(Tester.AdminGuid, Tester.GroismanGuid);
            Assert.True(res);
            //delete information from tester
            Tester._groismanRegistered = false;
            Tester.GroismanGuid = Guid.Empty;
        }

        [Test]
        public static void RemoveOfRegisteredUserAT2()
        {
            if (!Tester._initalized)
            {
                InitializationAT();
                Tester._initalized = true;
            }
            Tester.PBridge.Login(Tester.GuestGuid, "admin", "000000");
            if (!Tester._groismanRegistered)
            {
                UserAT.RegisterAT1();
                Tester._groismanRegistered = true;
            }
            //make groisman owner
            if(Tester._groismanShop.CompareTo(Guid.Empty) == 0)
            {
                RegisteredBuyerAT.CreationOfNewStoreByRegisteredUserAT();
            }
            bool res = Tester.PBridge.RemoveUser(Tester.AdminGuid, Tester.GroismanGuid);
            Assert.False(res);
            //no delete is needed here.
        }
        public static void RunAdminAT()
        {
            InitializationAT();
            RemoveOfRegisteredUserAT();
        }
    }
}
