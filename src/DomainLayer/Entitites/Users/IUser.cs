﻿using System;
using System.Collections.Generic;
using DomainLayer.Data.Entitites.Users.States;

namespace DomainLayer.Data.Entitites
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
        bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid);
        bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid);
        bool ConnectToPaymentSystem();
        bool ConnectToSupplySystem();
        bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount);
        void EditProductInShop(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        ICollection<Guid> GetAllProductsInCart(Guid shopGuid);
        ICollection<ShoppingBag> GetShoppingHistory();
        Guid OpenShop();
        bool PurchaseBag();
        bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid);
        bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid);
        bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid);
        bool RemoveUser(Guid userToRemoveGuid);
        ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType);
        #endregion
    }
}