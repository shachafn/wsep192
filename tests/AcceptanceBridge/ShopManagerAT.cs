using NUnit.Framework;
using ATBridge;
using ApplicationCore.Exceptions;
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
        [TearDown]
        public static void TearDown()
        {
            Tester.PBridge.ClearSystem();
        }

        //GR 5.1 - Commit something by manager should be done according to his permissions.

            //cookie tries to add benCookie and let him manage products
        [Test]
        public static void AddShopManagerAT_1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");
            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyManageProducts = { true, false, false, false };
            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, onlyManageProducts);
            var res = StoreOwnerAT.AddProductToShop(benCookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            Assert.AreNotEqual(Guid.Empty, res);

        }

        //cookie tries to add benCookie and let him control state of shop.
        [Test]
        public static void AddShopManagerAT_2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyControlShopState = { false, true, false, false };
            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, onlyManageProducts);
            Assert.NotThrows<NoPrivillgesException>(() => Tester.PBridge.CloseShop(benGuid, shopGuid));
        }

        //cookie tries to add benCookie and let him ManagePolicies.
        [Test]
        public static void AddShopManagerAT_3()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 5);
            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyManagePolicies = { false, false, true, false };
            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, onlyManagePolicies);
            Guid res = Tester.PBridge.AddNewPurchasePolicy(benGuid, shopGuid, "Product purchase policy", productGuid, "<", 2, "Must buy less than 2 Galaxy9");
            Assert.AreNotEqual(Guid.Empty, res);
        }

        //cookie tries to add benCookie and let him AppointManagers.
        [Test]
        public static void AddShopManagerAT_4()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyAppointManagers = { false, false, false, true };
            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, onlyAppointManagers);

            UserAT.GenerateRandoms(out var lironCookie, out var lironUsername, out var lironPassword);
            var lironGuid = UserAT.RegisterUser(lironCookie, lironUsername, lironPassword);
            bool res = Tester.PBridge.AddShopManager(benGuid, shopGuid, lironGuid, onlyAppointManagers);
            Assert.IsTrue(res);
        }

        //anotherCookie tries to add benCookie and let him AppointManagers in the shop of cookie.
        //should fail
        [Test]
        public static void AddShopManagerAT_5()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");

            UserAT.GenerateRandoms(out var anothercookie, out var anotherusername, out var anotherpassword);
            UserAT.RegisterUser(anothercookie, anotherusername, anotherpassword);
            UserAT.LoginUser(anothercookie, anotherusername, anotherpassword);

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyAppointManagers = { false, false, false, true };
            Assert.Throws<BadStateException>( ()=>Tester.PBridge.AddShopManager(anothercookie, shopGuid, benGuid, onlyAppointManagers));
        }

        //cookie tries to add benCookie and let him AppointManagers but ben thinks he is
        //a dictator and tries to do something else.
        [Test]
        public static void AddShopManagerAT_6()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyAppointManagers = { false, false, false, true };
            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, onlyAppointManagers);
            Assert.Throws<NoPrivillgesException>(() => Tester.PBridge.CloseShop(benGuid, shopGuid) / AddProductToShop(benCookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10));
        }


        //cookie tries to add benCookie and let him some responsibilties.
        [Test]
        public static void AddShopManagerAT_7()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            var shopGuid = Tester.PBridge.OpenShop(cookie, "Name");
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 5);
            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            List<bool> onlyManagePolicies = { true, false, true, false };
            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, onlyManagePolicies);
            Guid res = Tester.PBridge.AddNewPurchasePolicy(benGuid, shopGuid, "Product purchase policy", productGuid, "<", 2, "Must buy less than 2 Galaxy9");
            Assert.AreNotEqual(Guid.Empty, res);
            var res = StoreOwnerAT.AddProductToShop(benCookie, shopGuid, "Galaxy S10", "Cellphones", 2000, 15);
            Assert.AreNotEqual(Guid.Empty, res);
        }

    }
}
