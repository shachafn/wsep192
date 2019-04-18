using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Data.Entitites;

namespace ServiceLayer
{
    /// <summary>
    /// This is a proxy class to resolve session cookie before processing the request.
    /// </summary>
    public class ServiceFacadeProxy
    {
        public IServiceFacade _serviceFacade = ServiceFacade.Instance;
        public SessionManager _sessionManager = SessionManager.Instance;

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
            if (!isSuccess)
            {
                _sessionManager.SetLoggedOut(cookie);
                return true;
            }
            return false;
        }

        public Guid AddProductToShop(Guid cookie, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddProductToShop(userGuid, shopGuid, name, category, price, quantity);
        }

        public bool AddProductToShoppingCart(Guid cookie, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.AddProductToShoppingCart(userGuid, shopGuid, shopProductGuid, quantity);

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

        public bool EditShopProduct(Guid cookie, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.EditShopProduct(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid cookie, Guid shopGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.GetAllProductsInCart(userGuid, shopGuid);
        }

        public bool Initialize(Guid cookie, string username, string password)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.Initialize(userGuid, username, password);
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

        public bool Register(Guid cookie, string username, string password)
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
            return _serviceFacade.RemoveShopProduct(userGuid, shopGuid, shopProductGuid);
        }

        public bool RemoveShopManager(Guid cookie, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.RemoveShopManager(userGuid, shopGuid, managerToRemoveGuid);
        }

        public bool RemoveUser(Guid cookie, Guid userToRemoveGuid)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.RemoveUser(userGuid, userToRemoveGuid);
        }

        public ICollection<Guid> SearchProduct(Guid cookie, Guid shopGuid, string productName)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.SearchProduct(userGuid, shopGuid, productName);
        }

        public bool ChangeUserState(Guid cookie, string newState)
        {
            var userGuid = _sessionManager.ResolveCookie(cookie);
            return _serviceFacade.ChangeUserState(userGuid, newState);
        }
    }
}
