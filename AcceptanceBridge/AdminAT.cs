using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;

namespace Tests
{
    class AdminAT
    {
        ProxyBridge _proxy = new ProxyBridge();
        [SetUp]
        public void Setup()
        {
            _proxy.SetRealBridge(new BridgeImpl());
        }

        //GR 6.2 - Removing of registered user

        public void RemoveOfRegisteredUserAT()
        {
            RemoveOfRegisteredUserAT1();
            RemoveOfRegisteredUserAT2();
        }

        [Test]
        public void RemoveOfRegisteredUserAT1()
        {
            Assert.Pass();
        }

        [Test]
        public void RemoveOfRegisteredUserAT2()
        {
            Assert.Pass();
        }
        public void RunAdminAT()
        {
            RemoveOfRegisteredUserAT();
        }
    }
}
