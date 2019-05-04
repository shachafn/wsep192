using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using ApplicationCore.Data.Collections;
using ApplicationCore.Entitites;
using ApplicationCore.Entities.Users;

namespace ServiceLayer
{
    public class ServiceFacade 
    {
        IDomainLayerFacade _domainLayerFacade;
        ILogger<ServiceFacade> _logger;
        public ServiceFacade(IDomainLayerFacade domainLayerFacade, ILogger<ServiceFacade> logger)
        {
            _domainLayerFacade = domainLayerFacade;
            _logger = logger;
        }

        #region Users
        public Guid Register(UserIdentifier userIdentifier, string username, string password)
        {
            return _domainLayerFacade.Register(userIdentifier, username, password);
        }

        public Guid Login(UserIdentifier userIdentifier, string username, string password)
        {
            return _domainLayerFacade.Login(userIdentifier, username, password);
        }

        public bool Logout(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.Logout(userIdentifier);
        }
        #endregion

        #region Admin
        public Guid Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            return _domainLayerFacade.Initialize(userIdentifier, username, password);
        }
        public bool ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.ConnectToPaymentSystem(userIdentifier);
        }

        public bool ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.ConnectToSupplySystem(userIdentifier);
        }
        #endregion

        #region Shop Products

        public Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            return _domainLayerFacade.AddProductToShop(userIdentifier, shopGuid, name, category, price, quantity);
        }

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _domainLayerFacade.EditProductInShop(userIdentifier, shopGuid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            return _domainLayerFacade.RemoveProductFromShop(userIdentifier, shopGuid, shopProductGuid);
        }
        #endregion

        #region Shop Owners and Managers
        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            return _domainLayerFacade.AddShopManager(userIdentifier, shopGuid, newManagaerGuid, priviliges);
        }

        public bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return _domainLayerFacade.RemoveShopManager(userIdentifier, shopGuid, managerToRemoveGuid);
        }

        public bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid)
        {
            return _domainLayerFacade.AddShopOwner(userIdentifier, shopGuid, newShopOwnerGuid);
        }

        public bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return _domainLayerFacade.CascadeRemoveShopOwner(userIdentifier, shopGuid, ownerToRemoveGuid);
        }
        #endregion

        #region Shopping Cart
        public bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            return _domainLayerFacade.AddProductToCart(userIdentifier, shopGuid, shopProductGuid, quantity);
        }


        public bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            return _domainLayerFacade.RemoveProductFromCart(userIdentifier, shopGuid, shopProductGuid);
        }

        public bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return _domainLayerFacade.EditProductInCart(userIdentifier, shopGuid, shopProductGuid, newAmount);
        }

        public ICollection<Guid> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            return _domainLayerFacade.GetAllProductsInCart(userIdentifier, shopGuid);
        }
        #endregion

        #region Utils
        public IEnumerable<Shop> getUserShops(UserIdentifier userId)
        {
            return _domainLayerFacade.getUserShops(userId);
        }

        public IEnumerable<ShopProduct> getShopProducts(UserIdentifier userId,Guid shopGuid)
        {
            return _domainLayerFacade.GetShopProducts(userId, shopGuid);
        }
        #endregion

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.OpenShop(userIdentifier);
        }

        public bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            return _domainLayerFacade.PurchaseCart(userIdentifier, shopGuid);
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            return _domainLayerFacade.RemoveUser(userIdentifier, userToRemoveGuid);
        }

        //Product and shopGuid
        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            return _domainLayerFacade.SearchProduct(userIdentifier, toMatch, searchType);
        }

        public bool ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            return _domainLayerFacade.ChangeUserState(userIdentifier, newState);
        }

        public void ClearSystem()
        {
            _domainLayerFacade.ClearSystem();
        }

        public ICollection<Tuple<Guid, Product, int>> GetPurchaseHistory(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.GetPurchaseHistory(userIdentifier);
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.GetAllUsersExceptMe(userIdentifier);
        }

        public ICollection<Shop> GetAllShops(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.GetAllShops(userIdentifier);
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            _domainLayerFacade.CloseShopPermanently(userIdentifier, shopGuid);
        }
    }
}
