using DomainLayer.Data;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Domains;
using DomainLayer.Exceptions;
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

        public bool Register(Guid userGuid, string username, string password)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, username, password);
            return UserDomain.Register(username, password, false);
        }

        public Guid Login(Guid userGuid, string username, string password)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, username, password);
            return UserDomain.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return UserDomain.LogoutUser(userGuid);
        }

        public Guid OpenShop(Guid userGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return GetLoggedInUser(userGuid).OpenShop();
        }


        public bool PurchaseBag(Guid userGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            // Need to actually pay for products
            // if success clear all carts
            return GetLoggedInUser(userGuid).PurchaseBag();
        }

        public Guid Initialize(Guid userGuid, string username, string password)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, username, password);

            if (_isSystemInitialized)
                throw new SystemAlreadyInitializedException($"Cannot initialize the system again.");
            if (!External_Services.ExternalServicesManager._paymentSystem.IsAvailable())
                throw new ServiceUnReachableException($"Payment System Service is unreachable.");
            if (!External_Services.ExternalServicesManager._supplySystem.IsAvailable())
                throw new ServiceUnReachableException($"Supply System Service is unreachable.");

            var res = Guid.Empty;

            if (!UserDomain.IsAdminExists())
                UserDomain.Register(username, password, true);

            res = UserDomain.Login(userGuid, username, password);
            UserDomain.ChangeUserState(res, AdminUserState.AdminUserStateString);

            _isSystemInitialized = res.Equals(Guid.Empty) ? false : true;
            return res;
        }

        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return GetLoggedInUser(userGuid).ConnectToPaymentSystem();
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return GetLoggedInUser(userGuid).ConnectToSupplySystem();
        }

        public Guid AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, name, category, price, quantity);
            return GetLoggedInUser(userGuid).AddProductToShop(shopGuid, name, category, price, quantity);
        }

        public bool EditProductInShop(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, productGuid, newPrice, newQuantity);
            GetLoggedInUser(userGuid).EditProductInShop(shopGuid, productGuid, newPrice, newQuantity);
            return true;
        }

        public bool RemoveProductFromShop(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, shopProductGuid);
            return GetLoggedInUser(userGuid).RemoveProductFromShop(shopGuid, shopProductGuid);
        }

        public bool AddProductToCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopProductGuid, quantity);
            return GetLoggedInUser(userGuid).AddProductToCart(shopGuid, shopProductGuid, quantity);
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, newManagaerGuid, priviliges);
            return GetLoggedInUser(userGuid).AddShopManager(shopGuid, newManagaerGuid, priviliges);
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, ownerToRemoveGuid);
            return GetLoggedInUser(userGuid).CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, shopProductGuid, newAmount);
            return GetLoggedInUser(userGuid).EditProductInCart(shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, shopProductGuid);
            return GetLoggedInUser(userGuid).RemoveProductFromCart(shopGuid, shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid);
            return GetLoggedInUser(userGuid).GetAllProductsInCart(shopGuid);
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, userToRemoveGuid);
            return GetLoggedInUser(userGuid).RemoveUser(userToRemoveGuid);
        }

        public ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, productName);
            return GetShop(shopGuid).SearchProduct(productName);
        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, managerToRemoveGuid);
            return GetLoggedInUser(userGuid).RemoveShopManager(shopGuid, managerToRemoveGuid);
        }

        public bool ChangeUserState(Guid userGuid, string newState)
        {
            VerifySystemIsInitialized();
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), newState);
            return UserDomain.ChangeUserState(userGuid, newState);
        }

        public void ClearSystem()
        {
            DomainData.ClearAll();
            _isSystemInitialized = false;
        }

        private User GetLoggedInUser(Guid userGuid) => DomainData.LoggedInUsersEntityCollection[userGuid];
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
