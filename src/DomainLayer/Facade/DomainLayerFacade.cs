using DomainLayer.Data;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Domains;
using DomainLayer.Exceptions;
using DomainLayer.ExposedClasses;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static DomainLayer.Data.Entitites.Shop;

namespace DomainLayer.Facade
{
    public class DomainLayerFacade : IDomainLayerFacade
    {
        UserDomain UserDomain = UserDomain.Instance;

        #region Singleton Implementation
        private static IDomainLayerFacade instance = null;
        private static readonly object padlock = new object();
        public static IDomainLayerFacade Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DomainLayerFacade();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");
        private static bool _isSystemInitialized = false;

        public Guid Register(UserIdentifier userIdentifier, string username, string password)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            return UserDomain.Register(username, password, false);
        }

        public Guid Login(UserIdentifier userIdentifier, string username, string password)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            return UserDomain.Login(username, password);
        }

        public bool Logout(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return UserDomain.LogoutUser(userIdentifier);
        }

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return UserDomain.GetUserObject(userIdentifier).OpenShop();
        }


        public bool PurchaseBag(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            // Need to actually pay for products
            // if success clear all carts
            return UserDomain.GetUserObject(userIdentifier).PurchaseBag();
        }

        public Guid Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);

            if (_isSystemInitialized)
                throw new SystemAlreadyInitializedException($"Cannot initialize the system again.");
            if (!External_Services.ExternalServicesManager._paymentSystem.IsAvailable())
                throw new ServiceUnReachableException($"Payment System Service is unreachable.");
            if (!External_Services.ExternalServicesManager._supplySystem.IsAvailable())
                throw new ServiceUnReachableException($"Supply System Service is unreachable.");

            var res = Guid.Empty;

            if (!UserDomain.IsAdminExists())
                UserDomain.Register(username, password, true);

            res = UserDomain.Login(username, password);
            UserDomain.ChangeUserState(res, AdminUserState.AdminUserStateString);

            _isSystemInitialized = res.Equals(Guid.Empty) ? false : true;
            return res;
        }

        public bool ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return UserDomain.GetUserObject(userIdentifier).ConnectToPaymentSystem();
        }

        public bool ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return UserDomain.GetUserObject(userIdentifier).ConnectToSupplySystem();
        }

        public Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, name, category, price, quantity);
            return UserDomain.GetUserObject(userIdentifier).AddProductToShop(shopGuid, name, category, price, quantity);
        }

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity, string name, string category)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, productGuid, newPrice, newQuantity, name, category);
            UserDomain.GetUserObject(userIdentifier).EditProductInShop(shopGuid, productGuid, newPrice, newQuantity, name, category);
            return true;
        }

        public bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid);
            return UserDomain.GetUserObject(userIdentifier).RemoveProductFromShop(shopGuid, shopProductGuid);
        }

        public bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid, quantity);
            return UserDomain.GetUserObject(userIdentifier).AddProductToCart(shopGuid, shopProductGuid, quantity);
        }

        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, newManagaerGuid, priviliges);
            return UserDomain.GetUserObject(userIdentifier).AddShopManager(shopGuid, newManagaerGuid, priviliges);
        }

        public bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, newShopOwnerGuid);
            return UserDomain.GetUserObject(userIdentifier).AddShopOwner(shopGuid, newShopOwnerGuid);
        }

        public bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, ownerToRemoveGuid);
            return UserDomain.GetUserObject(userIdentifier).CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
        }

        public bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid, newAmount);
            return UserDomain.GetUserObject(userIdentifier).EditProductInCart(shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid);
            return UserDomain.GetUserObject(userIdentifier).RemoveProductFromCart(shopGuid, shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            return UserDomain.GetUserObject(userIdentifier).GetAllProductsInCart(shopGuid);
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, userToRemoveGuid);
            return UserDomain.GetUserObject(userIdentifier).RemoveUser(userToRemoveGuid);
        }

        public ICollection<Guid> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, toMatch, searchType);
            return UserDomain.GetUserObject(userIdentifier).SearchProduct(toMatch, searchType);
        }

        public bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, managerToRemoveGuid);
            return UserDomain.GetUserObject(userIdentifier).RemoveShopManager(shopGuid, managerToRemoveGuid);
        }

        public bool ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);
            return UserDomain.ChangeUserState(userIdentifier.Guid, newState);
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
    }
}
