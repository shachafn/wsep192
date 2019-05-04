using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.Extensions.Logging;

namespace ServiceLayer
{
    /// <summary>
    /// This is a proxy class to resolve session cookie before processing the request.
    /// </summary>
    public class ServiceFacadeProxy : IServiceFacade
    {
        public ServiceFacade _serviceFacade;
        public SessionManager _sessionManager;
        ILogger<ServiceFacadeProxy> _logger;
        public ServiceFacadeProxy(ServiceFacade serviceFacade, SessionManager sessionManager, ILogger<ServiceFacadeProxy> logger)
        {
            _serviceFacade = serviceFacade;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        // Login and Logout functions will act quite different becuase we
        // need them to maintain the Sessions mapping from cookie to user guid
        public bool Login(Guid cookie, string username, string password)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            var actualUserGuid = _serviceFacade.Login(userGuid, username, password);
            if (!actualUserGuid.Equals(Guid.Empty))
            {
                _sessionManager.SetLoggedIn(cookie, actualUserGuid);
                return true;
            }
            return false;
        }


        public bool Logout(Guid cookie)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            var isSuccess = _serviceFacade.Logout(userGuid);
            if (isSuccess)
            {
                _sessionManager.SetSessionLoggedOut(cookie);
                return true;
            }
            return false;
        }

        public bool RemoveUser(Guid cookie, Guid userToRemoveGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            var isSuccess = _serviceFacade.RemoveUser(userGuid, userToRemoveGuid);
            if (isSuccess)
            {
                _sessionManager.SetUserLoggedOut(userToRemoveGuid);
                return true;
            }
            return false;
        }

        public Guid Initialize(Guid cookie, string username, string password)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            var actualUserGuid = _serviceFacade.Initialize(userGuid, username, password);
            if (!actualUserGuid.Equals(Guid.Empty))
                _sessionManager.SetLoggedIn(cookie, actualUserGuid);

            return actualUserGuid;
        }

        public Guid AddProductToShop(Guid cookie, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddProductToShop(userGuid, shopGuid, name, category, price, quantity);
        }

        public bool AddProductToCart(Guid cookie, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddProductToCart(userGuid, shopGuid, shopProductGuid, quantity);

        }

        public bool AddShopManager(Guid cookie, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddShopManager(userGuid, shopGuid, newManagaerGuid, priviliges);
        }

        public bool AddShopOwner(Guid cookie, Guid shopGuid, Guid newShopOwnerGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddShopOwner(userGuid, shopGuid, newShopOwnerGuid);
        }

        public bool CascadeRemoveShopOwner(Guid cookie, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.CascadeRemoveShopOwner(userGuid, shopGuid, ownerToRemoveGuid);
        }

        public bool ConnectToPaymentSystem(Guid cookie)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.ConnectToPaymentSystem(userGuid);
        }

        public bool ConnectToSupplySystem(Guid cookie)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.ConnectToSupplySystem(userGuid);
        }

        public bool EditProductInCart(Guid cookie, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.EditProductInCart(userGuid, shopGuid, shopProductGuid, newAmount);
        }

        public bool EditProductInShop(Guid cookie, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.EditProductInShop(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid cookie, Guid shopGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.GetAllProductsInCart(userGuid, shopGuid);
        }

        public Guid OpenShop(Guid cookie)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.OpenShop(userGuid);
        }

        public bool PurchaseBag(Guid cookie)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.PurchaseBag(userGuid);
        }

        public bool PurchaseBag(Guid cookie, Guid shopGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.PurchaseCart(userGuid, shopGuid);
        }

        public Guid Register(Guid cookie, string username, string password)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.Register(userGuid, username, password);
        }

        public bool RemoveProductFromCart(Guid cookie, Guid shopGuid, Guid shopProductGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.RemoveProductFromCart(userGuid, shopGuid, shopProductGuid);
        }

        public bool RemoveProductFromShop(Guid cookie, Guid shopGuid, Guid shopProductGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.RemoveProductFromShop(userGuid, shopGuid, shopProductGuid);
        }

        public bool RemoveShopManager(Guid cookie, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.RemoveShopManager(userGuid, shopGuid, managerToRemoveGuid);
        }

        public ICollection<Guid> SearchProduct(Guid cookie, ICollection<string> toMatch, string searchType)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.SearchProduct(userGuid, toMatch, searchType);
        }

        public bool ChangeUserState(Guid cookie, string newState)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.ChangeUserState(userGuid, newState);
        }

        public void ClearSystem()
        {
            _sessionManager.Clear();
            _serviceFacade.ClearSystem();
        }

        public bool PurchaseCart(Guid cookie, Guid shopGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.PurchaseCart(userGuid, shopGuid);
        }

        public bool AddNewPurchasePolicy(Guid cookie, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddNewPurchasePolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
        }

        public bool AddNewDiscountPolicy(Guid cookie, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddNewDiscountPolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
        }
    }
}
