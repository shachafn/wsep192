using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;

namespace Tests
{
    public static class AdminAT
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
        }
        //GR 1 - Initialization of the system
        [Test]
        public static void InitializationAT()
        {
            var initResult = Tester.PBridge.Initialize(Tester.GuestGuid, "admin", "000000");
            Assert.AreNotEqual(initResult, Guid.Empty);
            Tester._initalized = true;
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
            //Initialize (registers if no admin user exists) and logs him in.
            Tester._groismanRegistered = true; 

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
                RegisteredBuyerAT.OpenStoreAT1();
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
