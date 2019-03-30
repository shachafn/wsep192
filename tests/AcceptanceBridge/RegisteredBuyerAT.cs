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
        private User _aUser;
        //GR 3.1 - Registered user can commit logout.
        [SetUp]
        public void SetUp()
        {
            _aUser =_proxy.Register("groisman","king98");
            
        }
        [Test]
        public void LogoutAT()
        {
            SetUp();
            Assert.AreEqual(false, _proxy.Logout()); //TODO: Change default expected value if needed.
        }

        //GR 3.2 - Registered user can open new store.
        [Test]
        public void CreationOfNewStoreByRegisteredUserAT()
        {
            SetUp();
            Assert.AreEqual(false,_proxy.OpenShop()); //TODO: Change expected value when imp will be available.
        }

        public void RunRegisteredUserAT()
        {
            LogoutAT();
            CreationOfNewStoreByRegisteredUserAT();
        }
    }
}
