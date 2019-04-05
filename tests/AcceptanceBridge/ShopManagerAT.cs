using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
namespace Tests
{
    [TestFixture]
    class ShopManagerAT
    {
        ProxyBridge _proxy = new ProxyBridge();
        [SetUp]
        public void Setup()
        {
            _proxy.SetRealBridge(new BridgeImpl());
        }
        
        //GR 5.1 - Commit something by manager should be done according to his permissions.
        [Test]
        public void CommitSomethingWithPermissionAT()
        {
            Assert.Pass();
        }
        public void RunShopManagerAT()
        {
            CommitSomethingWithPermissionAT();
        }
    }
}
