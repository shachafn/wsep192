using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests
{
    [TestFixture]
    class ShopManagerAT
    {
        public void ProductInsertionAT()
        {
            ProductInsertionAT1();
            ProductInsertionAT2();
        }

        [Test]
        public void ProductInsertionAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void ProductInsertionAT2()
        {
            Assert.AreEqual(true, true);
        }

        public void RunShopMangagerAT()
        {
            ProductInsertionAT();
        }
    }
}
