using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;

namespace ATBridge
{
    public class ProxyBridge : IBridge
    {
        private IBridge _real;

        public ProxyBridge()
        {
            _real = null;
        }

        public bool AddProductToCart(Guid userGuid, Guid shopGuid, Guid productGuid, int quantity)
        {
            return _real == null ? false : _real.AddProductToCart(userGuid, shopGuid, productGuid, quantity);
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            return _real == null ? false : _real.AddShopManager(userGuid, shopGuid, newManagaerGuid, privileges);
        }

        public bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid)
        {
            return _real == null ? false : _real.AddShopOwner(userGuid, shopGuid, newShopOwnerGuid);
        }

        public Guid AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            return _real == null ? Guid.Empty : _real.AddProductToShop(userGuid, shopGuid, name, category, price, quantity);
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return _real == null ? false : _real.CascadeRemoveShopOwner(userGuid, shopGuid, ownerToRemoveGuid);
        }

        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            return _real == null ? false : _real.ConnectToPaymentSystem(userGuid);
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            return _real == null ? false : _real.ConnectToSupplySystem(userGuid);
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return _real == null ? false : _real.EditProductInCart(userGuid, shopGuid, shopProductGuid, newAmount);
        }

        public bool EditProductInShop(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _real == null ? false : _real.EditProductInShop(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }

        public ICollection<ShopProduct> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            return _real?.GetAllProductsInCart(userGuid, shopGuid);
        }

        public Guid Initialize(Guid userGuid, string username, string password)
        {
            return _real == null ? Guid.Empty : _real.Initialize(userGuid, username, password);
        }

        public bool Login(Guid userGuid, string username, string password)
        {
            return _real == null ? false : _real.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            return _real == null ? false : _real.Logout(userGuid);
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            return _real == null ? false : _real.PurchaseCart(userGuid, shopGuid);
        }

        public Guid Register(Guid userGuid, string username, string password)
        {
            return _real == null ? Guid.Empty : _real.Register(userGuid, username, password);
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            return _real == null ? false : _real.RemoveProductFromCart(userGuid, shopGuid, shopProductGuid);
        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return _real == null ? false : _real.RemoveShopManager(userGuid, shopGuid, managerToRemoveGuid);
        }

        public bool RemoveProductFromShop(Guid userGuid, Guid shopProductGuid, Guid shopGuid)
        {
            return _real == null ? false : _real.RemoveProductFromShop(userGuid, shopProductGuid, shopGuid);
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            return _real == null ? false : _real.RemoveUser(userGuid, userToRemoveGuid);
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(Guid userGuid, ICollection<string> toMatch, string searchType)
        {
            return _real?.SearchProduct(userGuid, toMatch, searchType);
        }

        public bool ChangeUserState(Guid userGuid, string newState)
        {
            return _real == null ? false : _real.ChangeUserState(userGuid, newState);
        }

        public void SetRealBridge(IBridge impl)
        {
            if (_real == null)
                _real = impl;
        }

        public void ClearSystem()
        {
            _real.ClearSystem();
        }

        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null)
        {
            return _real == null ? Guid.Empty : _real.AddNewPurchasePolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
        }

        public Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null, object field5 = null)
        {
            return _real == null ? Guid.Empty : _real.AddNewDiscountPolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4, field5);
        }

        public Guid OpenShop(Guid cookie, string shopName)
        {
            return _real == null ? Guid.Empty : _real.OpenShop(cookie, shopName);
        }

        public void CloseShop(Guid cookie, Guid shopGuid)
        {
            if (_real != null)
                _real.CloseShop(cookie, shopGuid);
        }

        public void CloseShopPermanently(Guid cookie, Guid shopGuid)
        {
            if (_real != null)
                _real.CloseShopPermanently(cookie, shopGuid);
        }

        public void ReopenShop(Guid cookie, Guid shopGuid)
        {
            if (_real != null)
                _real.ReopenShop(cookie, shopGuid);
        }

        public Guid GetUserGuid(string ownerName)
        {
            return _real == null ? Guid.Empty :  _real.GetUserGuid(ownerName);
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(Guid cookie)
        {
            return _real == null ? null : _real.GetPurchaseHistory(cookie);
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(Guid cookie)
        {
            return _real == null ? null : _real.GetAllUsersExceptMe(cookie);
        }

        public IEnumerable<Shop> GetUserShops(Guid id)
        {
            return _real == null ? null : _real.GetUserShops(id);
        }

        public string GetUserName(Guid userGuid)
        {
            return _real == null ? null : _real.GetUserName(userGuid);
        }

        public IEnumerable<ShopProduct> GetShopProducts(Guid id, Guid shopGuid)
        {
            return _real == null ? null : _real.GetShopProducts(id, shopGuid);
        }

        public ICollection<Shop> GetAllShops(Guid cookie)
        {
            return _real == null ? null : _real.GetAllShops(cookie);
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag(Guid cookie)
        {
            return _real == null ? null : _real.GetUserBag(cookie);
        }

        public void cancelOwnerAssignment(Guid cookie, Guid shopId)
        {
            if (_real != null)
                _real.cancelOwnerAssignment(cookie, shopId);
        }
    }
}
