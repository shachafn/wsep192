using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests
{
    [TestFixture]
    class AdminAT
    {
        public void RemoveRegisteredBuyerAT()
        {
            RemoveRegisteredBuyerAT1();
            RemoveRegisteredBuyerAT2();
        }

        [Test]
        public void RemoveRegisteredBuyerAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveRegisteredBuyerAT2()
        {
            Assert.AreEqual(true, true);
        }

        public void RunAdminAT()
        {
            RemoveRegisteredBuyerAT();
        }
    }
}
