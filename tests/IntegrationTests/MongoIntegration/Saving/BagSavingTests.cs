using ApplicationCore.Entitites;
using ATBridge;
using DataAccessLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTests.MongoIntegration.Saving
{
    [TestFixture]
    public class BagSavingTests
    {
        static UnitOfWork _unitOfWork;
        static MongoDbContext _context;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            MongoIntegrationData.PBridge.SetRealBridge(new BridgeImpl());
            _unitOfWork = MongoIntegrationData.GetCurrentUnitOfWork();
            _context = MongoIntegrationData.GetCurrentContext();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
        }

        [TearDown]
        public static void TearDown()
        {
        }

        [SetUp]
        public static void SetUp()
        {
            //MongoIntegrationData.PBridge.ClearSystem();
        }


        [Test]
        public static void TestEmptyShoppingBag()
        {
            var bag = SaveEmptyShoppingBag();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();

            _unitOfWork.BagRepository.Add(bag);

            var savedBag = _unitOfWork.BagRepository.FindByIdOrNull(bag.Guid);

            VerifyEmptyShoppingBag(bag, savedBag);

            session.AbortTransaction();
        }
        public static ShoppingBag SaveEmptyShoppingBag()
        {
            ShoppingBag shop = new ShoppingBag(Guid.NewGuid());

            return shop;
        }

        [Test]
        public static void TestShoppingBag()
        {
            var bag = SaveShoppingBag();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();

            _unitOfWork.BagRepository.Add(bag);

            var savedBag = _unitOfWork.BagRepository.FindByIdOrNull(bag.Guid);

            VerifyShoppingBag(bag, savedBag);

            session.AbortTransaction();
        }

        public static ShoppingBag SaveShoppingBag()
        {
            ShoppingBag shop = new ShoppingBag(Guid.NewGuid());
            shop.ShoppingCarts.Add(new ShoppingCart(Guid.NewGuid(), Guid.NewGuid()));
            return shop;
        }

        [Test]
        public static void TestShoppingBagWithProducts()
        {
            var bag = SaveShoppingBagWithProducts();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();

            _unitOfWork.BagRepository.Add(bag);

            var savedBag = _unitOfWork.BagRepository.FindByIdOrNull(bag.Guid);

            VerifyShoppingBagWithProducts(bag, savedBag);

            session.AbortTransaction();
        }

        public static ShoppingBag SaveShoppingBagWithProducts()
        {
            ShoppingBag shop = new ShoppingBag(Guid.NewGuid());
            shop.ShoppingCarts.Add(new ShoppingCart(Guid.NewGuid(), Guid.NewGuid()));
            return shop;
        }

        #region Utils
        public static void VerifyShoppingBag(ShoppingBag expected, ShoppingBag actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.UserGuid, actual.UserGuid);
            CollectionAssert.IsNotEmpty(actual.ShoppingCarts);
            Assert.AreEqual(1, actual.ShoppingCarts.Count);
            VerifyCart(expected.ShoppingCarts.First(), actual.ShoppingCarts.First());
        }

        public static void VerifyEmptyShoppingBag(ShoppingBag expected, ShoppingBag actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.UserGuid, actual.UserGuid);
            CollectionAssert.IsEmpty(actual.ShoppingCarts);
        }

        private static void VerifyCart(ShoppingCart expected, ShoppingCart actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.UserGuid, actual.UserGuid);
            Assert.AreEqual(expected.ShopGuid, actual.ShopGuid);
            Assert.AreEqual(expected.PurchasedProducts.Count, actual.PurchasedProducts.Count);
            for (var i = 0; i < actual.PurchasedProducts.Count; i++)
            {
                var expec = expected.PurchasedProducts.ElementAt(i);
                var act = actual.PurchasedProducts.ElementAt(i);
                VerifyShopProduct(expec.Item1, act.Item1);
                Assert.AreEqual(expec.Item2, act.Item2);
            }
        }

        private static void VerifyShoppingBagWithProducts(ShoppingBag expected, ShoppingBag actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.UserGuid, actual.UserGuid);
            Assert.AreEqual(expected.ShoppingCarts.Count, actual.ShoppingCarts.Count);
            for (var i = 0; i < actual.ShoppingCarts.Count; i++)
            {
                var expec = expected.ShoppingCarts.ElementAt(i);
                var act = actual.ShoppingCarts.ElementAt(i);
                VerifyCart(expec, act);
            }
        }

        public static void VerifyShopProduct(ShopProduct expected, ShopProduct actual)
        {
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.Quantity, actual.Quantity);
            Assert.AreEqual(expected.Product.Name, actual.Product.Name);
            Assert.AreEqual(expected.Product.Category, actual.Product.Category);
            CollectionAssert.AreEquivalent(expected.Product.Keywords, actual.Product.Keywords);
        }
        #endregion
    }
}
