using ApplicationCore.Entitites;
using ATBridge;
using DataAccessLayer;
using DomainLayer.Operators;
using DomainLayer.Policies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTests.MongoIntegration.Saving
{
    [TestFixture]
    public class ShopSavingTests
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
        public static void TestSaveShopWithoutName()
        {
            var shop = SaveShopWithoutName();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();

            _unitOfWork.ShopRepository.Add(shop);

            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopWithoutName(shop, savedShop);

            session.AbortTransaction();
        }
        public static Shop SaveShopWithoutName()
        {
            Shop shop = new Shop(Guid.NewGuid());

            return shop;
        }

        public static void VerifyShopWithoutName(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
        }

        [Test]
        public static void TestSaveShopWithName()
        {
            var shop = SaveShopWithName();

            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopWithName(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopWithName()
        {
            var shop = SaveShopWithoutName();
            shop.ShopName = "MyShopName";

            return shop;
        }

        public static void VerifyShopWithName(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
        }

        [Test]
        public static void TestSaveShopOwner()
        {
            var shop = SaveShopOwner();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopOwner(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopOwner()
        {
            var shop = SaveShopWithName();

            var owner = new ShopOwner(Guid.NewGuid(), shop.Guid);
            shop.AddShopOwner(owner);

            return shop;
        }

        public static void VerifyShopOwner(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.Owners);
            Assert.AreEqual(1, actual.Owners.Count);
            VerifyOwner(expected.Owners.First(), actual.Owners.First());
        }

        [Test]
        public static void TestSaveShopManager()
        {
            var shop = SaveShopManager();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopManager(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopManager()
        {
            var shop = SaveShopWithName();

            var manager = new ShopOwner(Guid.NewGuid(), shop.Guid, new List<bool>() { true, false, false, false });
            shop.AddShopManager(manager);

            return shop;
        }

        public static void VerifyShopManager(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.Managers);
            Assert.AreEqual(1, actual.Managers.Count);
            VerifyOwner(expected.Managers.First(), actual.Managers.First());
        }



        [Test]
        public static void TestSaveShopProducts()
        {
            var shop = SaveShopProducts();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopProducts(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopProducts()
        {
            var shop = SaveShopWithName();

            var galaxy = new ShopProduct(new Product("Galaxy", "Smartphones"), 10, 10);
            shop.AddProductToShop(galaxy);
            return shop;
        }

        public static void VerifyShopProducts(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.ShopProducts);
            Assert.AreEqual(1, actual.ShopProducts.Count);
            VerifyShopProduct(expected.ShopProducts.First(), actual.ShopProducts.First());
        }

        [Test]
        public static void TestSaveShopState()
        {
            var shop = SaveShopState();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopState(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopState()
        {
            var shop = SaveShopWithName();
            shop.ShopState = Shop.ShopStateEnum.PermanentlyClosed;
            return shop;
        }

        public static void VerifyShopState(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Shop.ShopStateEnum.PermanentlyClosed, actual.ShopState);
        }

        [Test]
        public static void TestSaveShopUsersPurchaseHistory()
        {
            var shop = SaveShopUsersPurchaseHistory();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopUsersPurchaseHistory(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopUsersPurchaseHistory()
        {
            var shop = SaveShopWithName();
            var galaxy = new ShopProduct(new Product("Galaxy", "Smartphones"), 10, 10);
            shop.UsersPurchaseHistory.Add(new Tuple<Guid, ShopProduct, int>(Guid.NewGuid(), galaxy, 5));
            return shop;
        }

        public static void VerifyShopUsersPurchaseHistory(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.UsersPurchaseHistory);
            Assert.AreEqual(1, actual.UsersPurchaseHistory.Count);
            VerifyUsersPurchaseHistory(expected.UsersPurchaseHistory.First(), actual.UsersPurchaseHistory.First());
        }

        [Test]
        public static void TestSaveProductPurchasePolicy()
        {
            var shop = SaveShopProductPurchasePolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopProductPurchasePolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopProductPurchasePolicy()
        {
            var shop = SaveShopWithName();
            var policy = new ProductPurchasePolicy(Guid.NewGuid(), new SmallerThan(), 1, "desc");
            shop.PurchasePolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopProductPurchasePolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.PurchasePolicies);
            Assert.AreEqual(1, actual.PurchasePolicies.Count);
            VerifyProductPurchasePolicy(expected.PurchasePolicies.First(), actual.PurchasePolicies.First());
        }


        [Test]
        public static void TestSaveShopUserPurchasePolicy()
        {
            var shop = SaveShopUserPurchasePolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopUserPurchasePolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopUserPurchasePolicy()
        {
            var shop = SaveShopWithName();
            var policy = new UserPurchasePolicy("field", 1, "desc");
            shop.PurchasePolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopUserPurchasePolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.PurchasePolicies);
            Assert.AreEqual(1, actual.PurchasePolicies.Count);
            VerifyUserPurchasePolicy(expected.PurchasePolicies.First(), actual.PurchasePolicies.First());
        }


        [Test]
        public static void TestSaveShopCartPurchasePolicy()
        {
            var shop = SaveShopCartPurchasePolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopCartPurchasePolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopCartPurchasePolicy()
        {
            var shop = SaveShopWithName();
            var policy = new CartPurchasePolicy(5, new SmallerThan(), "desc");
            shop.PurchasePolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopCartPurchasePolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.PurchasePolicies);
            Assert.AreEqual(1, actual.PurchasePolicies.Count);
            VerifyCartPurchasePolicy(expected.PurchasePolicies.First(), actual.PurchasePolicies.First());
        }

        [Test]
        public static void TestSaveShopCompositePurchasePolicy()
        {
            var shop = SaveShopCompositePurchasePolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopCompositePurchasePolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopCompositePurchasePolicy()
        {
            var shop = SaveShopWithName();
            var policy = new CompositePurchasePolicy
            (
                new CartPurchasePolicy(5, new SmallerThan(), "desc"),
                new Or(),
                new UserPurchasePolicy("field", 1, "desc"),
                "desc"
            );
            shop.PurchasePolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopCompositePurchasePolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.PurchasePolicies);
            Assert.AreEqual(1, actual.PurchasePolicies.Count);
            VerifyCompositePurchasePolicy(expected.PurchasePolicies.First(), actual.PurchasePolicies.First());
        }

        [Test]
        public static void TestSaveShopUserDiscountPolicy()
        {
            var shop = SaveShopUserDiscountPolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopUserDiscountPolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopUserDiscountPolicy()
        {
            var shop = SaveShopWithName();
            var policy = new UserDiscountPolicy("field", 1, 1, "desc");
            shop.DiscountPolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopUserDiscountPolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.DiscountPolicies);
            Assert.AreEqual(1, actual.DiscountPolicies.Count);
            VerifyUserDiscountPolicy(expected.DiscountPolicies.First(), actual.DiscountPolicies.First());
        }

        [Test]
        public static void TestSaveCartDiscountPolicy()
        {
            var shop = SaveShopCartDiscountPolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopCartDiscountPolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopCartDiscountPolicy()
        {
            var shop = SaveShopWithName();
            var policy = new CartDiscountPolicy(new SmallerThan(), 2.3, 1, "desc");
            shop.DiscountPolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopCartDiscountPolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.DiscountPolicies);
            Assert.AreEqual(1, actual.DiscountPolicies.Count);
            VerifyCartDiscountPolicy(expected.DiscountPolicies.First(), actual.DiscountPolicies.First());
        }

        [Test]
        public static void TestSaveShopProductDiscountPolicy()
        {
            var shop = SaveShopProductDiscountPolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopProductDiscountPolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopProductDiscountPolicy()
        {
            var shop = SaveShopWithName();
            var policy = new ProductDiscountPolicy(Guid.NewGuid(), new SmallerThan(), 1, 2, "desc");
            shop.DiscountPolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopProductDiscountPolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.DiscountPolicies);
            Assert.AreEqual(1, actual.DiscountPolicies.Count);
            VerifyProductDiscountPolicy(expected.DiscountPolicies.First(), actual.DiscountPolicies.First());
        }

        [Test]
        public static void TestSaveCompositeDiscountPolicy()
        {
            var shop = SaveShopCompositeDiscountPolicy();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopCompositeDiscountPolicy(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopCompositeDiscountPolicy()
        {
            var shop = SaveShopWithName();
            var policy = new CompositeDiscountPolicy
            (
                new ProductDiscountPolicy(Guid.NewGuid(), new SmallerThan(), 1, 2, "desc"),
                new Or(),
                new CartDiscountPolicy(new SmallerThan(), 2.3, 1, "desc"),
                24,
                "desc"
            );
            shop.DiscountPolicies.Add(policy);
            return shop;
        }

        public static void VerifyShopCompositeDiscountPolicy(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            CollectionAssert.IsNotEmpty(actual.DiscountPolicies);
            Assert.AreEqual(1, actual.DiscountPolicies.Count);
            VerifyCompositeDiscountPolicy(expected.DiscountPolicies.First(), actual.DiscountPolicies.First());
        }

        [Test]
        public static void TestSaveShopOwnerCandidate()
        {
            var shop = SaveShopOwnerCandidate();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            _unitOfWork.ShopRepository.Add(shop);
            var savedShop = _unitOfWork.ShopRepository.FindByIdOrNull(shop.Guid);

            VerifyShopOwnerCandidate(shop, savedShop);
            session.AbortTransaction();
        }
        public static Shop SaveShopOwnerCandidate()
        {
            var shop = SaveShopWithName();
            var candidate = new OwnerCandidate(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2, "sa");
            shop.candidate = candidate;
            candidate.Signatures.Add("hi", Guid.NewGuid());
            return shop;
        }

        public static void VerifyShopOwnerCandidate(Shop expected, Shop actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.ShopName, actual.ShopName);
            Assert.NotNull(actual.Creator);
            Assert.AreEqual(expected.Creator.OwnerGuid, actual.Creator.OwnerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            Assert.AreEqual(Guid.Empty, actual.Creator.AppointerGuid);
            VerifyOwnerCandidate(expected.candidate, actual.candidate);
        }

        
        #region Utils
        public static void VerifyOwner(ShopOwner expected, ShopOwner actual)
        {
            Assert.AreEqual(expected.OwnerGuid, actual.OwnerGuid);
            Assert.AreEqual(expected.AppointerGuid, actual.AppointerGuid);
            Assert.AreEqual(expected.ShopGuid, actual.ShopGuid);
            CollectionAssert.AreEquivalent(expected.Privileges, actual.Privileges);
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

        public static void VerifyUsersPurchaseHistory(Tuple<Guid, ShopProduct, int> expected, Tuple<Guid, ShopProduct, int> actual)
        {
            Assert.AreEqual(expected.Item1, actual.Item1);
            VerifyShopProduct(expected.Item2, actual.Item2);
            Assert.AreEqual(expected.Item3, actual.Item3);
        }

        public static void VerifyProductPurchasePolicy(IPurchasePolicy purchasePolicy1, IPurchasePolicy purchasePolicy2)
        {
            var expected = (ProductPurchasePolicy)purchasePolicy1;
            var actual = (ProductPurchasePolicy)purchasePolicy2;
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ProductGuid, actual.ProductGuid);
            Assert.AreEqual(expected.ExpectedQuantity, actual.ExpectedQuantity);
            Assert.AreEqual(expected.Operator.ToString(), actual.Operator.ToString());
        }

        private static void VerifyUserPurchasePolicy(IPurchasePolicy purchasePolicy1, IPurchasePolicy purchasePolicy2)
        {
            var expected = (UserPurchasePolicy)purchasePolicy1;
            var actual = (UserPurchasePolicy)purchasePolicy2;
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(expected.FieldName, actual.FieldName);
        }

        private static void VerifyCartPurchasePolicy(IPurchasePolicy purchasePolicy1, IPurchasePolicy purchasePolicy2)
        {
            var expected = (CartPurchasePolicy)purchasePolicy1;
            var actual = (CartPurchasePolicy)purchasePolicy2;
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Operator.ToString(), actual.Operator.ToString());
            Assert.AreEqual(expected.ExpectedQuantity, actual.ExpectedQuantity);
        }

        private static void VerifyCompositePurchasePolicy(IPurchasePolicy purchasePolicy1, IPurchasePolicy purchasePolicy2)
        {
            var expected = (CompositePurchasePolicy)purchasePolicy1;
            var actual = (CompositePurchasePolicy)purchasePolicy2;

            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Operator.ToString(), actual.Operator.ToString());
            VerifyCartPurchasePolicy(expected.PurchasePolicy1, actual.PurchasePolicy1);
            VerifyUserPurchasePolicy(expected.PurchasePolicy2, actual.PurchasePolicy2);
        }

        private static void VerifyUserDiscountPolicy(IDiscountPolicy discountPolicy1, IDiscountPolicy discountPolicy2)
        {
            var expected = (UserDiscountPolicy)discountPolicy1;
            var actual = (UserDiscountPolicy)discountPolicy2;

            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.FieldName, actual.FieldName);
            Assert.AreEqual(expected.ExpectedValue, actual.ExpectedValue);
            Assert.AreEqual(expected.DiscountPercentage, actual.DiscountPercentage);
        }

        private static void VerifyCartDiscountPolicy(IDiscountPolicy discountPolicy1, IDiscountPolicy discountPolicy2)
        {
            var expected = (CartDiscountPolicy)discountPolicy1;
            var actual = (CartDiscountPolicy)discountPolicy2;

            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ExpectedSum, actual.ExpectedSum);
            Assert.AreEqual(expected.Operator.ToString(), actual.Operator.ToString());
            Assert.AreEqual(expected.DiscountPercentage, actual.DiscountPercentage);
        }

        private static void VerifyProductDiscountPolicy(IDiscountPolicy discountPolicy1, IDiscountPolicy discountPolicy2)
        {
            var expected = (ProductDiscountPolicy)discountPolicy1;
            var actual = (ProductDiscountPolicy)discountPolicy2;

            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ProductGuid, actual.ProductGuid);
            Assert.AreEqual(expected.Operator.ToString(), actual.Operator.ToString());
            Assert.AreEqual(expected.DiscountPercentage, actual.DiscountPercentage);
        }

        private static void VerifyCompositeDiscountPolicy(IDiscountPolicy purchasePolicy1, IDiscountPolicy purchasePolicy2)
        {
            var expected = (CompositeDiscountPolicy)purchasePolicy1;
            var actual = (CompositeDiscountPolicy)purchasePolicy2;

            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Operator.ToString(), actual.Operator.ToString());
            Assert.AreEqual(expected.DiscountPercentage, actual.DiscountPercentage);
            VerifyProductDiscountPolicy(expected.DiscountPolicy1, actual.DiscountPolicy1);
            VerifyCartDiscountPolicy(expected.DiscountPolicy2, actual.DiscountPolicy2);
        }

        private static void VerifyOwnerCandidate(OwnerCandidate expected, OwnerCandidate actual)
        {
            Assert.AreEqual(expected.AppointerGuid, actual.AppointerGuid);
            Assert.AreEqual(expected.OwnerGuid, actual.OwnerGuid);
            Assert.AreEqual(expected.ShopGuid, actual.ShopGuid);
            CollectionAssert.AreEquivalent(expected.Signatures, actual.Signatures);
            Assert.AreEqual(expected.signature_target, actual.signature_target);
        }

        #endregion

    }
}
