using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
using DomainLayer.Exceptions;

namespace Tests
{
    [TestFixture]
    public static class AdminAT
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
        }

        [TearDown]
        public static void TearDown()
        {
            Tester.PBridge.ClearSystem();
        }

        [SetUp]
        public static void SetUp()
        {
            Tester.AdminGuid = Tester.PBridge.Initialize(Tester.GuestGuid, "admin", "000000");
        }

        #region GR 1 - Initialization of the system

        [Test]
        public static void InitializationAT()
        {
            Assert.AreNotEqual(Tester.AdminGuid, Guid.Empty);
        }

        #endregion

        #region GR 6.2 - Removing of a registered user

        [Test]
        public static void RemoveOfRegisteredUserAT1()
        {
            UserAT.RegisterAT1();
            bool res = Tester.PBridge.RemoveUser(Tester.AdminGuid, Tester.GroismanGuid);
            Assert.True(res);
            //delete information from tester
            Tester._groismanRegistered = false;
            Tester.GroismanGuid = Guid.Empty;
        }

        [Test]
        public static void RemoveOfRegisteredUserAT2()
        {
            if (!Tester._groismanRegistered)
            {
                UserAT.LoginAT1();
                Tester._groismanRegistered = true;
            }
            //make groisman owner
            if (Tester._groismanShop.Equals(Guid.Empty))
                RegisteredBuyerAT.OpenStoreAT1();

            Assert.Throws<BrokenConstraintException>(
                () => Tester.PBridge.RemoveUser(Tester.AdminGuid, Tester.GroismanGuid));
        }

        #endregion

    }
}
