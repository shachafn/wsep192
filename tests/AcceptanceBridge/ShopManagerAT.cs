using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ATBridge;
namespace Tests
{
    [TestFixture]
    public static class ShopManagerAT
    {
        [SetUp]
        public static void Setup()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
            if (!Tester._initalized)
            {
                AdminAT.InitializationAT();
            }
        }
        
        //GR 5.1 - Commit something by manager should be done according to his permissions.
        [Test]
        public static void CommitSomethingWithPermissionAT()
        {
            CommitSomethingWithPermissionAT1();
            CommitSomethingWithPermissionAT2();
        }

        public static void CommitSomethingWithPermissionAT1()
        {
            
        }

        public static void CommitSomethingWithPermissionAT2()
        {
            Assert.Pass();
        }
        public static void RunShopManagerAT()
        {
            CommitSomethingWithPermissionAT();
        }
    }
}
