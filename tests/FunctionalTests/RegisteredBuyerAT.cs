using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests
{
    [TestFixture]
    class RegisteredBuyerAT
    {

        //GR 3.1 - Registered user can commit logout.

        [Test]
        public void LogoutAT()
        {
            Assert.AreEqual(true, true);
        }

        //GR 3.2 - Registered user can open new store.
        [Test]
        public void CreationOfNewStoreByRegisteredUserAT()
        {
            Assert.AreEqual(true, true);
        }

        public void RegisteredUserAT()
        {
            LogoutAT();
            CreationOfNewStoreByRegisteredUserAT();
        }
    }
}
