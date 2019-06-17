using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.ServiceLayer;
using ApplicationCore.Entitites;
using TestsUtils;
using ApplicationCore.Entities.Users;

namespace ATBridge
{
    public class BridgeImpl : IBridge
    {
        private readonly IServiceFacade _serviceFacade;

        public BridgeImpl()
        {
            _serviceFacade = MocksCreator.GetServiceFacade();
        }

        public bool AddProductToCart(Guid userGuid, Guid shopGuid, Guid productGuid, int quantity)
        {
            return _serviceFacade.AddProductToCart(userGuid, shopGuid, productGuid, quantity);
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            return _serviceFacade.AddShopManager(userGuid, shopGuid, newManagaerGuid, privileges);

        }

        public bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid)
        {
            return _serviceFacade.AddShopOwner(userGuid, shopGuid, newShopOwnerGuid);
        }

        public Guid AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            return _serviceFacade.AddProductToShop(userGuid, shopGuid, name, category, price, quantity);
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return _serviceFacade.CascadeRemoveShopOwner(userGuid, shopGuid, ownerToRemoveGuid);
        }

        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            return _serviceFacade.ConnectToSupplySystem(userGuid);
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            return _serviceFacade.ConnectToSupplySystem(userGuid);
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return _serviceFacade.EditProductInCart(userGuid, shopGuid, shopProductGuid, newAmount);
        }

        public bool EditProductInShop(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _serviceFacade.EditProductInShop(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }

        public ICollection<ShopProduct> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            return _serviceFacade.GetAllProductsInCart(userGuid, shopGuid);
        }

        public Guid Initialize(Guid userGuid, string username, string password)
        {
            return _serviceFacade.Initialize(userGuid, username, password);
        }

        public bool Login(Guid userGuid, string username, string password)
        {
            return _serviceFacade.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            return _serviceFacade.Logout(userGuid);
        }

        public Guid OpenShop(Guid userGuid)
        {
            return _serviceFacade.OpenShop(userGuid, string.Empty);
        }

        public Guid OpenShop(Guid userGuid, string shopName)
        {
            return _serviceFacade.OpenShop(userGuid, shopName);
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            return _serviceFacade.PurchaseCart(userGuid, shopGuid);
        }
        public double GetCartPrice(Guid userGuid, Guid shopGuid)
        {
            return _serviceFacade.GetCartPrice(userGuid, shopGuid);
        }

        public Guid Register(Guid userGuid, string username, string password)
        {
            return _serviceFacade.Register(userGuid, username, password);
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            return _serviceFacade.RemoveProductFromCart(userGuid, shopGuid, shopProductGuid);

        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return _serviceFacade.RemoveShopManager(userGuid, shopGuid, managerToRemoveGuid);
        }

        public bool RemoveProductFromShop(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            return _serviceFacade.RemoveProductFromShop(userGuid, shopGuid, shopProductGuid);
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            return _serviceFacade.RemoveUser(userGuid, userToRemoveGuid);
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(Guid userGuid, ICollection<string> toMatch, string searchType)
        {
            return _serviceFacade.SearchProduct(userGuid, toMatch, searchType);
        }

        public bool ChangeUserState(Guid userGuid, string newState)
        {
            return _serviceFacade.ChangeUserState(userGuid, newState);
        }

        public void ClearSystem()
        {
            _serviceFacade.ClearSystem();
        }

        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null)
        {
            return _serviceFacade.AddNewPurchasePolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
        }

        public Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null, object field5 = null)
        {
            return _serviceFacade.AddNewDiscountPolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4, field5);
        }

        public void CloseShop(Guid coolie, Guid shopGuid)
        {
            _serviceFacade.CloseShop(coolie, shopGuid);
        }

        public void CloseShopPermanently(Guid cookie, Guid shopGuid)
        {
            _serviceFacade.CloseShopPermanently(cookie, shopGuid);
        }

        public void ReopenShop(Guid cookie, Guid shopGuid)
        {
            _serviceFacade.ReopenShop(cookie, shopGuid);
        }

        public Guid GetUserGuid(string ownerName)
        {
            return _serviceFacade.GetUserGuid(ownerName);
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(Guid cookie)
        {
            return _serviceFacade.GetPurchaseHistory(cookie);
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(Guid cookie)
        {
            return _serviceFacade.GetAllUsersExceptMe(cookie);
        }

        public IEnumerable<Shop> GetUserShops(Guid id)
        {
            return _serviceFacade.GetUserShops(id);
        }

        public string GetUserName(Guid userGuid)
        {
            return _serviceFacade.GetUserName(userGuid);
        }

        public IEnumerable<ShopProduct> GetShopProducts(Guid id, Guid shopGuid)
        {
            return _serviceFacade.GetShopProducts(id, shopGuid);
        }

        public ICollection<Shop> GetAllShops(Guid cookie)
        {
            return _serviceFacade.GetAllShops(cookie);
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag(Guid cookie)
        {
            return _serviceFacade.GetUserBag(cookie);
        }

        public void CancelOwnerAssignment(Guid cookie, Guid shopId)
        {
            _serviceFacade.CancelOwnerAssignment(cookie, shopId);
        }
    }
}
