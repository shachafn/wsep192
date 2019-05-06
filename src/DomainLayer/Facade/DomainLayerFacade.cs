using ApplicationCore.Data;
using ApplicationCore.Data.Collections;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Events;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DomainLayer;
using DomainLayer.Extension_Methods;
using DomainLayer.Policies;
using DomainLayer.Users.States;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DomainLayer.Facade
{
    public class DomainLayerFacade : IDomainLayerFacade
    {
        IUserDomain _userDomain;
        DomainLayerFacadeVerifier _verifier;
        ILogger<DomainLayerFacade> _logger;

        public DomainLayerFacade(IUserDomain userDomain, DomainLayerFacadeVerifier verifier
            , ILogger<DomainLayerFacade> logger)
        {
            _userDomain = userDomain;
            _verifier = verifier;
            _logger = logger;
        }

        private static bool _isSystemInitialized = false;

        public Guid Register(UserIdentifier userIdentifier, string username, string password)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            return _userDomain.Register(username, password, false);
        }

        public Guid Login(UserIdentifier userIdentifier, string username, string password)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            var result = _userDomain.Login(username, password);
            if (!result.Equals(Guid.Empty))
                UpdateCenter.RaiseEvent(new UserLoggedInEvent(result));
            return result;
        }

        public bool Logout(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.LogoutUser(userIdentifier);
        }

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.GetUserObject(userIdentifier).OpenShop();
        }


        public bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            // Need to actually pay for products
            // if success clear all carts
            return _userDomain.GetUserObject(userIdentifier).PurchaseCart(shopGuid);
        }

        public Guid Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);

            if (_isSystemInitialized)
                throw new SystemAlreadyInitializedException($"Cannot initialize the system again.");
            if (!External_Services.ExternalServicesManager._paymentSystem.IsAvailable())
                throw new ServiceUnReachableException($"Payment System Service is unreachable.");
            if (!External_Services.ExternalServicesManager._supplySystem.IsAvailable())
                throw new ServiceUnReachableException($"Supply System Service is unreachable.");

            var res = Guid.Empty;

            if (!_userDomain.IsAdminExists())
                _userDomain.Register(username, password, true);

            res = _userDomain.Login(username, password);
            _userDomain.ChangeUserState(res, AdminUserState.AdminUserStateString);

            _isSystemInitialized = res.Equals(Guid.Empty) ? false : true;
            return res;
        }

        public bool ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.GetUserObject(userIdentifier).ConnectToPaymentSystem();
        }

        public bool ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.GetUserObject(userIdentifier).ConnectToSupplySystem();
        }

        public Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, name, category, price, quantity);
            return _userDomain.GetUserObject(userIdentifier).AddProductToShop(shopGuid, name, category, price, quantity);
        }

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, productGuid, newPrice, newQuantity);
            _userDomain.GetUserObject(userIdentifier).EditProductInShop(shopGuid, productGuid, newPrice, newQuantity);
            return true;
        }

        public bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid);
            return _userDomain.GetUserObject(userIdentifier).RemoveProductFromShop(shopGuid, shopProductGuid);
        }

        public bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid, quantity);
            return _userDomain.GetUserObject(userIdentifier).AddProductToCart(shopGuid, shopProductGuid, quantity);
        }

        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, newManagaerGuid, priviliges);
            return _userDomain.GetUserObject(userIdentifier).AddShopManager(shopGuid, newManagaerGuid, priviliges);
        }

        public bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, newShopOwnerGuid);
            return _userDomain.GetUserObject(userIdentifier).AddShopOwner(shopGuid, newShopOwnerGuid);
        }

        public bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, ownerToRemoveGuid);
            var result = _userDomain.GetUserObject(userIdentifier).CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
            if (result) //Maybe change CascadeRemoveShopOwner in IUser to return a collection of all removed owners.
                UpdateCenter.RaiseEvent(new RemovedOwnerEvent(ownerToRemoveGuid, shopGuid));
            return result;
        }

        public bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid, newAmount);
            return _userDomain.GetUserObject(userIdentifier).EditProductInCart(shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid);
            return _userDomain.GetUserObject(userIdentifier).RemoveProductFromCart(shopGuid, shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            return _userDomain.GetUserObject(userIdentifier).GetAllProductsInCart(shopGuid);
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, userToRemoveGuid);
            return _userDomain.GetUserObject(userIdentifier).RemoveUser(userToRemoveGuid);
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, toMatch, searchType);
            return _userDomain.GetUserObject(userIdentifier).SearchProduct(toMatch, searchType);
        }

        public bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, managerToRemoveGuid);
            return _userDomain.GetUserObject(userIdentifier).RemoveShopManager(shopGuid, managerToRemoveGuid);
        }

        public ICollection<Tuple<Guid, Product, int>> GetPurchaseHistory(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.GetUserObject(userIdentifier).GetPurchaseHistory();
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.GetAllUsersExceptMe(userIdentifier);
        }

        public ICollection<Shop> GetAllShops(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return DomainData.ShopsCollection.ToList();
        }

        public IEnumerable<Shop> getUserShops(UserIdentifier userId)
        {
            /*todo verofy constraints
             * VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);*/
            return ApplicationCore.Data.DomainData.ShopsCollection.Where(shop => shop.Creator.OwnerGuid.Equals(userId.Guid)).ToList<Shop>();
        }

        public IEnumerable<ShopProduct> GetShopProducts(UserIdentifier userIdentifier, Guid shopGuid)
        {
            /*todo verofy constraints
            * VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);*/
            Shop shop = GetShop(shopGuid);
            return shop.ShopProducts;
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            /*todo verofy constraints
             * VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);*/
            Shop shop = GetShop(shopGuid);
            shop.AdminClose();
        }

        public bool ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);
            return _userDomain.ChangeUserState(userIdentifier.Guid, newState);
        }

        public void ClearSystem()
        {
            DomainData.ClearAll();
            _isSystemInitialized = false;
        }

        private Shop GetShop(Guid shopGuid) => DomainData.ShopsCollection[shopGuid];

        private void VerifySystemIsInitialized()
        {
            if (!_isSystemInitialized)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"System has not been initialized." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw new SystemNotInitializedException(msg);
            }
        }

        public Guid AddNewDiscountPolicy(UserIdentifier userIdentifier, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4,object field5)
        {
            VerifySystemIsInitialized();
            IUser user = _userDomain.GetUserObject(userIdentifier);
            IDiscountPolicy newPolicy = new UserDiscountPolicy();
            _verifier.AddNewDiscountPolicy(ref newPolicy, userIdentifier,shopGuid, policyType, field1, field2, field3,field4,field5);
            return user.AddNewDiscountPolicy(user.Guid, shopGuid, newPolicy);
        }

        public Guid AddNewPurchasePolicy(UserIdentifier userIdentifier, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4)
        {
            VerifySystemIsInitialized();
            IUser user = _userDomain.GetUserObject(userIdentifier);
            IPurchasePolicy newPolicy = new UserPurchasePolicy();
            _verifier.AddNewPurchasePolicy(ref newPolicy, userIdentifier,shopGuid, policyType, field1, field2, field3,field4);
            return user.AddNewPurchasePolicy(user.Guid, shopGuid, newPolicy);
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> getUserBag(UserIdentifier userIdentifier)
        {
           var bag = DomainData.ShoppingBagsCollection[userIdentifier.Guid] ;
            List<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> result = new List<Tuple<ShoppingCart, IEnumerable<ShopProduct>>>();
            if (bag != null && bag.ShoppingCarts != null)
            {
                foreach (var cart in bag.ShoppingCarts)
                {
                    List<ShopProduct> products = new List<ShopProduct>();
                    var shop = DomainData.ShopsCollection[cart.ShopGuid];
                    foreach (var item in cart.PurchasedProducts)
                    {
                        ShopProduct currProduct = shop.ShopProducts.FirstOrDefault(prod => prod.Guid.Equals(item.Item1));
                        ShopProduct product = new ShopProduct();
                        product.Product = new Product(currProduct.Product.Name, currProduct.Product.Category);
                        product.Guid = currProduct.Guid;
                        product.Price = currProduct.Price;
                        product.Quantity = item.Item2;
                        products.Add(product);
                    }
                    result.Add(new Tuple<ShoppingCart, IEnumerable<ShopProduct>>(cart, products));
                }
            }
            return result;
        }
    }
}
