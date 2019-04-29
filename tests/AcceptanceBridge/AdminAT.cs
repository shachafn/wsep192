using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
using ApplicationCore.Exceptions;

namespace Tests
{
    [TestFixture]
    public static class AdminAT
    {

        static Guid _adminCookie = Guid.NewGuid();
        static string _adminUsername = "admin";
        static string _adminPassword = "000000";

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
            Tester.AdminGuid = Tester.PBridge.Initialize(_adminCookie, _adminUsername, _adminPassword);
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
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            var userGuid = UserAT.RegisterUser(cookie, username, password);
            var res = Tester.PBridge.RemoveUser(_adminCookie, userGuid);
            Assert.True(res);
        }

        [Test]
        public static void RemoveOfRegisteredUserAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            var userGuid = UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            Tester.PBridge.OpenShop(cookie);

            Assert.Throws<BrokenConstraintException>(
                () => Tester.PBridge.RemoveUser(_adminCookie, userGuid));
        }

        #endregion

    }
}
