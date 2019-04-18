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
            Assert.Pass();
        }

        [Test]
        public static void RemoveOfRegisteredUserAT2()
        {
            Assert.Pass();
        }
        public static void RunAdminAT()
        {
            InitializationAT();
            RemoveOfRegisteredUserAT();
        }
    }
}
