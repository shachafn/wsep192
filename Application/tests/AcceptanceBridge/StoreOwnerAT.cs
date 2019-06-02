using System;
using System.Collections.Generic;
using NUnit.Framework;
using ATBridge;
using ApplicationCore.Exceptions;

namespace Tests
{
    [TestFixture]
    public static class StoreOwnerAT
    {
        static Guid _adminCookie = Guid.NewGuid();
        static string _adminUsername = "admin";
        static string _adminPassword = "000000";

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            Tester.PBridge.SetRealBridge(new BridgeImpl());
        }


        [TearDown]
        public static void TearDown()
        {
            Tester.PBridge.ClearSystem();
        }

        [SetUp]
        public static void SetUp()
        {
            Tester.AdminGuid = Tester.PBridge.Initialize(_adminCookie, _adminUsername, _adminPassword);
        }


        #region GR 4.1.1 - Store owner can add products

        [Test]
        public static void AddingProductAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var res = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            Assert.AreNotEqual(Guid.Empty, res);
        }

        [Test]
        public static void AddingProductAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            Tester.PBridge.ChangeUserState(cookie, "BuyerUserState");
            Assert.Throws<BadStateException>(
                () => AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10));
        }

        [Test]
        public static void AddingProductAT3()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);

            //non-owner user tries to add
            UserAT.GenerateRandoms(out var otherCookie, out var otherUsername, out var otherPassword);
            UserAT.RegisterUser(otherCookie, otherUsername, otherPassword);
            UserAT.LoginUser(otherCookie, otherUsername, otherPassword);
            Tester.PBridge.ChangeUserState(otherCookie, "SellerUserState");

            Assert.Throws<NoPriviligesException>(
                () => AddProductToShop(otherCookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10));
        }

        [Test]
        public static void AddingProductAT4()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            Assert.Throws<IllegalArgumentException>(
                () => AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", -2000, 10));
        }
        #endregion

        #region GR 4.1.2 - Shop owner can remove products
        //GR 4.1.2
        public static void RemovingProductAT()
        {
            RemovingProductAT1();
            RemovingProductAT2();
            RemovingProductAT3();
        }

        [Test]
        public static void RemovingProductAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            var res = Tester.PBridge.RemoveProductFromShop(cookie, shopGuid, productGuid);
            Assert.True(res);
        }

        [Test]
        public static void RemovingProductAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            Tester.PBridge.ChangeUserState(cookie, "BuyerUserState");
            Assert.Throws<BadStateException>(
                () => Tester.PBridge.RemoveProductFromShop(cookie, shopGuid, productGuid));
        }

        [Test]
        public static void RemovingProductAT3()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            Assert.Throws<ProductNotFoundException>(
                () => Tester.PBridge.RemoveProductFromShop(cookie, shopGuid, Guid.Empty));
        }

        #endregion

        #region GR 4.1.3 - Shop owner can edit products
        //GR 4.1.3 
        public static void EditingProductAT()
        {
            EditingProductAT1();
            EditingProductAT2();
            EditingProductAT3();
            EditingProductAT4();
        }

        [Test]
        public static void EditingProductAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var galaxyGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            bool result = Tester.PBridge.EditProductInShop(cookie, shopGuid, galaxyGuid, 1500, 20);
            Assert.True(result);
        }

        [Test]
        public static void EditingProductAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var galaxyGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            Tester.PBridge.ChangeUserState(cookie, "BuyerUserState");
            Assert.Throws<BadStateException>(() => Tester.PBridge.EditProductInShop(cookie, shopGuid, galaxyGuid, 1500, 20));
        }

        [Test]
        public static void EditingProductAT3()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var galaxyGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            //Empty guid does not match any product
            Assert.Throws<ProductNotFoundException>(() => Tester.PBridge.EditProductInShop(cookie, shopGuid, Guid.Empty, 1500, 20));
        }

        [Test]
        public static void EditingProductAT4()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var galaxyGuid = AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 10);
            Assert.Throws<IllegalArgumentException>(
                () => Tester.PBridge.EditProductInShop(cookie, shopGuid, galaxyGuid, 1500, -20));
        }
        #endregion

        #region GR 4.2.1 - Shop owner can define purchase policy in his store

        [Test]
        public static void AddPurchasePolicyAT1_1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 4);
            Guid res = Tester.PBridge.AddNewPurchasePolicy(cookie, shopGuid, "Product purchase policy", productGuid, "<", 2, "Must buy less than 2 Galaxy9");
            Assert.AreNotEqual(Guid.Empty, res);
        }

        [Test]
        public static void AddPurchasePolicyAT1_CompoundPolicy()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 5);
            var productGuid1 = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Iphone X", "Cellphones", 3000, 7);
            var policyGuid = Tester.PBridge.AddNewPurchasePolicy(cookie, shopGuid, "Product purchase policy", productGuid, ">", 1, "Must buy more than 1 Galaxy9");
            var policyGuid1 = Tester.PBridge.AddNewPurchasePolicy(cookie, shopGuid, "Product purchase policy", productGuid1, ">", 2, "Must buy more than 2 Iphone X");
            Guid res = Tester.PBridge.AddNewPurchasePolicy(cookie, shopGuid, "Compound purchase policy", policyGuid, "^", policyGuid1, "Must buy more than 1 Galaxy9 XOR more than 2 Iphone X");
            Assert.AreNotEqual(Guid.Empty, res);
        }

        [Test]
        public static void AddPurchasePolicyAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 1);
            UserAT.GenerateRandoms(out var anotherCookie, out var anotherUsername, out var anotherPassword);
            Assert.Throws<BadStateException>(() => Tester.PBridge.AddNewPurchasePolicy(anotherCookie, shopGuid, "Product purchase policy", productGuid, "<", 2, "Must buy less than 2 Galaxy9"));
        }

        #endregion
        #region GR 4.2.2 Shop owner can define discount policy in his store
        [Test]
        public static void AddDiscountPolicyAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 1);
            Guid res = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Product discount policy", productGuid, ">", 2, 40, "40% if you buy more than 2 Galaxy9");
            Assert.AreNotEqual(Guid.Empty, res);
        }
        [Test]
        public static void AddDiscountPolicyAT1_CompoundPolicy()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productHammer = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Hammer", "Tools", 100, 5);
            var productScrewDriver = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Screwdriver", "Tools", 90, 7);
            var productNail = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Nail", "Tools", 5, 100);
            var productDisc = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Disc", "Tools", 200, 4);
            var policyHammer = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Product discount policy", productHammer, ">", 0, 0, "Buy Hammer");
            var policyScrewDriver = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Product discount policy", productScrewDriver, ">", 0, 0, "Buy Screwdriver");
            var policyHammerAndScrew = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Compound discount policy", policyHammer, "&", policyScrewDriver,0, "Buy Hammer and Screwdriver");
            var policyDisc = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Product discount policy", productDisc, ">", 0, 0, "Buy Disc");
            var policyHammerAndScrew_OrDisc = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Compound discount policy", policyHammerAndScrew, "|", policyDisc, 0, "Buy (Hammer and Screwdriver) or Disc");
            var policyNail = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Product discount policy", productNail, ">", 0, 0, "Buy Nails");
            var policyHammerAndScrew_OrDisc_impliesNail = Tester.PBridge.AddNewDiscountPolicy(cookie, shopGuid, "Compound discount policy", policyHammerAndScrew_OrDisc, "->", policyNail, 60, "Buy (Hammer and Screwdriver) or Disc implies 60% on Nails");
            Assert.AreNotEqual(Guid.Empty, policyHammerAndScrew_OrDisc_impliesNail);
        }

        [Test]
        public static void AddDiscountPolicyAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            var productGuid = Tester.PBridge.AddProductToShop(cookie, shopGuid, "Galaxy S9", "Cellphones", 2000, 1);
            UserAT.GenerateRandoms(out var anotherCookie, out var anotherUsername, out var anotherPassword);
            Assert.Throws<BadStateException>(() => Tester.PBridge.AddNewDiscountPolicy(anotherCookie, shopGuid, "Product discount policy", productGuid, ">", 0, 20, "20% on Galaxy S9"));
        }





        #endregion

        #region GR 4.3 - Store's owner can appoint new owner to his store.

        [Test]
        public static void AppointmentOfNewOwnerAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);

            var res = Tester.PBridge.AddShopOwner(cookie, shopGuid, benGuid);
            Assert.True(res);
        }

        [Test]
        public static void AppointmentOfNewOwnerAT2()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            Assert.Throws<UserNotFoundException>(
                () => Tester.PBridge.AddShopOwner(cookie, shopGuid, Guid.Empty));
        }

        [Test]
        public static void AppointmentOfNewOwnerAT3()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);
            Tester.PBridge.ChangeUserState(cookie, "BuyerUserState");

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);

            Assert.Throws<BadStateException>(
                () => Tester.PBridge.AddShopOwner(cookie, shopGuid, benGuid));
        }
        #endregion
        //CAN NOT TEST THAT!
        /*[Test]
        public static void AppointmentOfNewOwnerAT4()
        {
            Assert.Pass();
        }*/


        #region GR 4.4 - Store's owner can remove new owner from his store.

        [Test]
        public static void RemoveOfOwnerAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);

            Tester.PBridge.AddShopOwner(cookie, shopGuid, benGuid);
            var res = Tester.PBridge.CascadeRemoveShopOwner(cookie, shopGuid, benGuid);
            Assert.IsTrue(res);
        }

        [Test]
        public static void RemoveOfOwnerAT2()
        {
            Assert.Throws<UserNotFoundException>(() => Tester.PBridge.CascadeRemoveShopOwner(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid));
        }

        [Test]
        public static void RemoveOfOwnerAT4()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);

            //Appoint ben
            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);
            UserAT.LoginUser(benCookie, benUsername, benPassword);
            Tester.PBridge.ChangeUserState(benCookie, "SellerUserState");

            Tester.PBridge.AddShopOwner(cookie, shopGuid, benGuid);

            //Ben appoints tom
            UserAT.GenerateRandoms(out var tomCookie, out var tomUsername, out var tomPassword);
            var tomGuid = UserAT.RegisterUser(tomCookie, tomUsername, tomPassword);

            Tester.PBridge.AddShopOwner(benCookie, shopGuid, tomGuid);

            //Remove ben, and cascade remove tom
            var res = Tester.PBridge.CascadeRemoveShopOwner(cookie, shopGuid, benGuid);
            Assert.IsTrue(res);

            UserAT.LoginUser(tomCookie, tomUsername, tomPassword);
            Tester.PBridge.ChangeUserState(benCookie, "SellerUserState");

            //verify he really is not an owner anymore
            Assert.Throws<NoPriviligesException>(
                () => Tester.PBridge.AddShopOwner(tomCookie, shopGuid, benGuid));
        }
        #endregion

        #region  GR 4.5 - Store's owner can appoint new owner to his store.

        [Test]
        public static void AppointmentOfNewManagerAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);

            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, new List<string>());
        }

        [Test]
        public static void AppointmentOfNewManagerAT2()
        {
            Assert.Throws<UserNotFoundException>(() => Tester.PBridge.AddShopManager(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid, new List<string>()));
        }

        #endregion

        #region  GR 4.6 - Store's owner can remove manager from his store.

        [Test]
        public static void RemoveOfManagerAT1()
        {
            UserAT.GenerateRandoms(out var cookie, out var username, out var password);
            UserAT.RegisterUser(cookie, username, password);
            UserAT.LoginUser(cookie, username, password);
            Tester.PBridge.ChangeUserState(cookie, "SellerUserState");
            var shopGuid = Tester.PBridge.OpenShop(cookie);

            UserAT.GenerateRandoms(out var benCookie, out var benUsername, out var benPassword);
            var benGuid = UserAT.RegisterUser(benCookie, benUsername, benPassword);

            Tester.PBridge.AddShopManager(cookie, shopGuid, benGuid, new List<string>());
            var res = Tester.PBridge.RemoveShopManager(cookie, shopGuid, benGuid);
            Assert.IsTrue(res);
        }

        [Test]
        public static void RemoveOfManagerAT2()
        {
            Assert.Throws<UserNotFoundException>(() => Tester.PBridge.RemoveShopManager(Tester.GroismanGuid, Tester._groismanShop, Tester.GuestGuid));
        }

        #endregion

        public static Guid AddProductToShop(Guid cookie, Guid shopGuid, string name = "name", string category = "category"
            , double price = 2, int quantity = 2)
        {
            return Tester.PBridge.AddProductToShop(cookie, shopGuid, name, category, price, quantity);
        }
    }
}
