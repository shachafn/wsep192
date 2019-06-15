using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
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

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            return _domainLayerFacade.RemoveUser(userIdentifier, userToRemoveGuid);
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
        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            return _domainLayerFacade.AddShopManager(userIdentifier, shopGuid, newManagaerGuid, privileges);
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

        public void CloseShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            _domainLayerFacade.CloseShop(userIdentifier, shopGuid);
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            _domainLayerFacade.CloseShopPermanently(userIdentifier, shopGuid);
        }

        public Guid OpenShop(UserIdentifier userIdentifier, string shopName)
        {
            return _domainLayerFacade.OpenShop(userIdentifier, shopName);
        }

        public void ReopenShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            _domainLayerFacade.ReopenShop(userIdentifier, shopGuid);
        }

        internal Guid AddNewDiscountPolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4, object field5)
        {
            return _domainLayerFacade.AddNewDiscountPolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4, field5);
        }

        internal Guid AddNewPurchasePolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4)
        {
            return _domainLayerFacade.AddNewPurchasePolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
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

        public ICollection<ShopProduct> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            return _domainLayerFacade.GetAllProductsInCart(userIdentifier, shopGuid);
        }

        public bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            return _domainLayerFacade.PurchaseCart(userIdentifier, shopGuid);
        }
        #endregion

        #region Utils
        public IEnumerable<Shop> GetUserShops(UserIdentifier userId)
        {
            return _domainLayerFacade.GetUserShops(userId);
        }

        public string GetUserName(Guid userGuid)
        {
            return _domainLayerFacade.GetUserName(userGuid);
        }

        public Guid GetUserGuid(string ownerName)
        {
            return _domainLayerFacade.GetUserGuid(ownerName);
        }

        public string GetShopName(Guid shopGuid)
        {
            return _domainLayerFacade.GetShopName(shopGuid);
        }

        public Guid GetShopGuid(string shopName)
        {
            return _domainLayerFacade.GetShopGuid(shopName);
        }

        public IEnumerable<ShopProduct> GetShopProducts(UserIdentifier userId,Guid shopGuid)
        {
            return _domainLayerFacade.GetShopProducts(userId, shopGuid);
        }
        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.getUserBag(userIdentifier);
        }
        #endregion

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

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(UserIdentifier userIdentifier)
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

        public void cancelOwnerAssignment(UserIdentifier userIdentifier, Guid shopId)
        {
             _domainLayerFacade.cancelOwnerAssignment(userIdentifier,shopId);
        }

        internal bool IsUserAdmin(Guid id)
        {
            return _domainLayerFacade.IsUserAdmin(id);
        }
    }
}
