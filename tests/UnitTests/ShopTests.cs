using DomainLayer.Data.Entitites;
using DomainLayer.Domains;
using DomainLayer.ExposedClasses;
using DomainLayer.Facade;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static DomainLayer.Data.Entitites.Shop;

namespace UnitTests
{
    [TestFixture]
    public class ShopTests
    {
        IDomainLayerFacade facade = DomainLayerFacade.Instance;
        UserDomain userDomain = UserDomain.Instance;

        UserIdentifier _idnetifier = default(UserIdentifier);
        const string _adminUsername = "admin";
        const string _adminPassword = "000000";
        Guid _adminGuid = Guid.Empty;

        Shop _shop = null;
        Guid _product1Guid = Guid.Empty;
        Guid _product2Guid = Guid.Empty;
        Product _product1 = null;
        Product _product2 = null;

        [SetUp]
        public void SetUp()
        {
            _idnetifier = new UserIdentifier(Guid.NewGuid(), true);
            _adminGuid = facade.Initialize(_idnetifier, _adminUsername, _adminPassword);
            _idnetifier = default(UserIdentifier);
        }

        [TearDown]
        public void TearDown()
        {
            facade.ClearSystem();
            _adminGuid = Guid.Empty;
            _shop = null;
            _product1Guid = Guid.Empty;
            _product2Guid = Guid.Empty;
            _product1 = null;
            _product2 = null;
        }

        [Test, Description("Test Shop Init")]
        public void TestShopInit()
        {
            _shop = new Shop(_adminGuid);
            Assert.AreEqual(_adminGuid, _shop.Creator.OwnerGuid);
            CollectionAssert.IsEmpty(_shop.Owners);
            CollectionAssert.IsEmpty(_shop.Managers);
            CollectionAssert.IsEmpty(_shop.ShopProducts);
            CollectionAssert.IsEmpty(_shop.UsersPurchaseHistory);
            Assert.AreEqual(ShopStateEnum.Active, _shop.ShopState);
        }


        [Test, Description("Test Shop States")]
        public void TestShopStates()
        {
            TestShopInit();
            _shop.Close();
            Assert.AreEqual(ShopStateEnum.Closed, _shop.ShopState);
            _shop.Open();
            Assert.AreEqual(ShopStateEnum.Active, _shop.ShopState);
            _shop.AdminClose();
            Assert.AreEqual(ShopStateEnum.PermanentlyClosed, _shop.ShopState);
        }

        [Test, Description("Test Shop Products Add")]
        public void TestShopAddProducts()
        {
            TestShopInit();
            _product1 = new Product("Iphone", "Cellphones");
            _product1Guid = _shop.AddProductToShop(_adminGuid, _product1, 99.5, 1);
            Assert.IsTrue(
                Enumerable.SequenceEqual(
                    new List<ShopProduct>() { new ShopProduct(_product1, 99.5, 1) },
                    _shop.ShopProducts)); 

            _product2 = new Product("Galaxy", "Cellphones");
            _product2Guid = _shop.AddProductToShop(_adminGuid, _product2, 99.5, 1);
            Assert.IsTrue(
                Enumerable.SequenceEqual(
                    new List<ShopProduct>() { new ShopProduct(_product1, 99.5, 1), new ShopProduct(_product2, 99.5, 1) },
                    _shop.ShopProducts));
        }

        [Test, Description("Test Shop Products Edit")]
        public void TestShopEditProducts()
        {
            TestShopAddProducts();
            _shop.EditProductInShop(_adminGuid, _product1Guid, 200, 11, "Iphone", "Cellphones");
            Assert.IsTrue(
    Enumerable.SequenceEqual(
        new List<ShopProduct>() { new ShopProduct(_product1, 200, 11), new ShopProduct(_product2, 99.5, 1) },
        _shop.ShopProducts));

            _shop.EditProductInShop(_adminGuid, _product2Guid, 99.5, 1, "Iphone", "Tech");
            _product2.Category = "Tech";
            Assert.IsTrue(
    Enumerable.SequenceEqual(
        new List<ShopProduct>() { new ShopProduct(_product1, 200, 11), new ShopProduct(_product2, 99.5, 1) },
        _shop.ShopProducts));
        }

        [Test, Description("Test Shop Products Remove")]
        public void TestShopRemoveProducts()
        {
            TestShopAddProducts();
            _shop.RemoveProductFromShop(_adminGuid, _product2Guid);
            Assert.IsTrue(
    Enumerable.SequenceEqual(
        new List<ShopProduct>() { new ShopProduct(_product1, 99.5, 1) },
        _shop.ShopProducts));

            _shop.RemoveProductFromShop(_adminGuid, _product1Guid);
            CollectionAssert.IsEmpty(_shop.ShopProducts);
        }

    }


}
