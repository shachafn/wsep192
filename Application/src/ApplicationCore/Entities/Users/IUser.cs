﻿using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using DomainLayer.Policies;

namespace ApplicationCore.Entities.Users
{
    public interface IUser
    {
        #region Properties
        Guid Guid { get; }
        bool IsAdmin { get; }
        #endregion

        #region Public Methods
        bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity);
        Guid AddProductToShop(Guid shopGuid, string name, string category, double price, int quantity);
        bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<bool> privileges);
        bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid);
        bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid);
        bool ConnectToPaymentSystem();
        bool ConnectToSupplySystem();
        bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount);
        void EditProductInShop(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        ICollection<ShopProduct> GetAllProductsInCart(Guid shopGuid);
        ICollection<Guid> GetShoppingHistory();
        Guid OpenShop();
        Guid OpenShop(string name);
        void ReopenShop(Guid shopGuid);
        void CloseShop(Guid shopGuid);
        void CloseShopPermanently(Guid shopGuid);
        bool PurchaseCart(Guid shopGuid);
        bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid);
        bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid);
        bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid);
        bool RemoveUser(Guid userToRemoveGuid);
        ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType);
        bool SetState(IAbstractUserState newState);
        Guid AddNewPurchasePolicy(Guid userGuid , Guid shopGuid, IPurchasePolicy newPolicy);
        Guid AddNewDiscountPolicy(Guid userGuid ,Guid shopGuid, IDiscountPolicy newPolicy);
        ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory();
        #endregion
    }
}