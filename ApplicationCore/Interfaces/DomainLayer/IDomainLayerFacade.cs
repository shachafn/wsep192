using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces.DomainLayer
{
    /// <summary>
    /// Exposes the Domain Layer's functionality, should not expose internal structures.
    /// If really needed, can expose new structures here and then return them.
    /// </summary>
    public interface IDomainLayerFacade
    {
        Guid Register(UserIdentifier userIdentifier, string username, string password);
        Guid Login(UserIdentifier userIdentifier, string username, string password);
        bool Logout(UserIdentifier userIdentifier);
        Guid OpenShop(UserIdentifier userIdentifier);
        bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid);
        Guid Initialize(UserIdentifier userIdentifier, string username, string password);
        bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid);
        bool ConnectToPaymentSystem(UserIdentifier userIdentifier);
        bool ConnectToSupplySystem(UserIdentifier userIdentifier);
        Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity);
        bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid);
        bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity);
        bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid);
        bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount);
        bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid);
        ICollection<Guid> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid);
        ICollection<Tuple<ShopProduct, Guid>> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType);
        bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid);
        bool ChangeUserState(UserIdentifier userIdentifier, string newState);
        void ClearSystem();
        bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid);
        ICollection<Tuple<Guid, Product, int>> GetPurchaseHistory(UserIdentifier userIdentifier);
        ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier);
        IEnumerable<ShopProduct> GetShopProducts(UserIdentifier userId, Guid shopGuid);
        IEnumerable<Shop> getUserShops(UserIdentifier userId);
        ICollection<Shop> GetAllShops(UserIdentifier userIdentifier);
        void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid);
        IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> getUserBag(UserIdentifier userIdentifier);
        bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid);
        Guid AddNewDiscountPolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4,object field5);
        Guid AddNewPurchasePolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4);
    }
}
