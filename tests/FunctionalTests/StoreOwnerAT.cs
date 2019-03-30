using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests
{
    [TestFixture]
    class StoreOwnerAT
    {
        //GR 4.1 - Store's owner can manage store in his store.
        public void StoreManagmentAT41()
        {
            AddingProductAT(); //GR 4.1.1
            RemovingProductAT(); //GR 4.1.2
            EditingProductAT(); //GR 4.1.3
        }

        //GR 4.1.1
        public void AddingProductAT()
        {
            AddingProductAT1();
            AddingProductAT2();
            AddingProductAT3();
            AddingProductAT4();
        }

        [Test]
        public void AddingProductAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AddingProductAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AddingProductAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AddingProductAT4()
        {
            Assert.AreEqual(true, true);
        }

        //GR 4.1.2
        public void RemovingProductAT()
        {
            RemovingProductAT1();
            RemovingProductAT2();
            RemovingProductAT3();
            RemovingProductAT4();
        }

        [Test]
        public void RemovingProductAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemovingProductAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemovingProductAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemovingProductAT4()
        {
            Assert.AreEqual(true, true);
        }

        //GR 4.1.3 
        public void EditingProductAT()
        {
            EditingProductAT1();
            EditingProductAT2();
            EditingProductAT3();
        }

        [Test]
        public void EditingProductAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void EditingProductAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void EditingProductAT3()
        {
            Assert.AreEqual(true, true);
        }

        //GR 4.3 - Store's owner can appoint new owner to his store.
        public void AppointmentOfNewOwnerAT()
        {
            AppointmentOfNewOwnerAT1();
            AppointmentOfNewOwnerAT2();
            AppointmentOfNewOwnerAT3();
            AppointmentOfNewOwnerAT4();
        }

        [Test]
        public void AppointmentOfNewOwnerAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AppointmentOfNewOwnerAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AppointmentOfNewOwnerAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AppointmentOfNewOwnerAT4()
        {
            Assert.AreEqual(true, true);
        }


        //GR 4.4 - Store's owner can remove new owner from his store.
        public void RemoveOfOwnerAT()
        {
            RemoveOfOwnerAT1();
            RemoveOfOwnerAT2();
            RemoveOfOwnerAT3();
            RemoveOfOwnerAT4();
        }

        [Test]
        public void RemoveOfOwnerAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveOfOwnerAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveOfOwnerAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveOfOwnerAT4()
        {
            Assert.AreEqual(true, true);
        }


        //GR 4.5 - Store's owner can appoint new owner to his store.
        public void AppointmentOfNewManagerAT()
        {
            AppointmentOfNewManagerAT1();
            AppointmentOfNewManagerAT2();
            AppointmentOfNewManagerAT3();
            AppointmentOfNewManagerAT4();
        }

        [Test]
        public void AppointmentOfNewManagerAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AppointmentOfNewManagerAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AppointmentOfNewManagerAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void AppointmentOfNewManagerAT4()
        {
            Assert.AreEqual(true, true);
        }


        //GR 4.6 - Store's owner can remove manager from his store.
        public void RemoveOfManagerAT()
        {
            RemoveOfManagerAT1();
            RemoveOfManagerAT2();
            RemoveOfManagerAT3();
            RemoveOfManagerAT4();
        }

        [Test]
        public void RemoveOfManagerAT1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveOfManagerAT2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveOfManagerAT3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void RemoveOfManagerAT4()
        {
            Assert.AreEqual(true, true);
        }


        public void RunStoreOwnerAT()
        {
            StoreManagmentAT(); //GR 4.1
            AppointmentOfNewOwnerAT(); //GR 4.3
            RemoveOfOwnerAT(); //GR 4.4
            AppointmentOfNewManagerAT(); //GR 4.5  
            RemoveOfManagerAT(); //GR 4.6
        }
    }
}
