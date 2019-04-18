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
            UserAT.RegisterAT1();
            UserAT.LoginAT1();
        }

        [Test]
        public static void LogoutAT()
        {
            if (Tester._groismanConnected)
            {
                Assert.IsTrue(Tester.PBridge.Logout(Tester.GroismanGuid));
                Tester._groismanConnected = false;
            }
        }

        //GR 3.2 - Registered user can open new store.
        [Test]
        public static void CreationOfNewStoreByRegisteredUserAT()
        {
            UserAT.LoginAT();
            //Assert.NotNull((Tester._groismanShop = _proxy.OpenShop("groisman")));
        }

        public static void RunRegisteredUserAT()
        {
            CreationOfNewStoreByRegisteredUserAT();
            LogoutAT();
            
        }
    }
}
