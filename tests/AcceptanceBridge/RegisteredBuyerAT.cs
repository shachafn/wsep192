using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DomainLayer;
using ATBridge;

namespace Tests
{
    [TestFixture]
    class RegisteredBuyerAT
    {
        ProxyBridge _proxy = new ProxyBridge();
        private UserAT _userAT = new UserAT();
        Guid _guid = Guid.NewGuid();

        //GR 3.1 - Registered user can commit logout.

        [SetUp]
        public void SetUp()
        {
            //_proxy = new ProxyBridge();
            _proxy.SetRealBridge(new BridgeImpl());
            
        }

        [Test]
        public void LogoutAT()
        {
            if (Tester._groismanConnected)
            {
                Assert.IsTrue(_proxy.Logout("groisman"));
                Tester._groismanConnected = false;
            }
        }

        //GR 3.2 - Registered user can open new store.
        [Test]
        public void CreationOfNewStoreByRegisteredUserAT()
        {
            _userAT.LoginAT();
            Assert.NotNull((Tester._groismanShop = _proxy.OpenShop("groisman")));
        }

        public void RunRegisteredUserAT()
        {
            CreationOfNewStoreByRegisteredUserAT();
            LogoutAT();
            
        }
    }
}
