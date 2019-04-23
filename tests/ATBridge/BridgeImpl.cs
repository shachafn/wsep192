﻿using System;
using System.Collections.Generic;
using ServiceLayer;
using DomainLayer.Data.Entitites;

namespace ATBridge
{
    public class BridgeImpl : IBridge
    {
        private readonly ServiceFacadeProxy _serviceFacade;

        public BridgeImpl()
        {
            _serviceFacade = new ServiceFacadeProxy();
        }

        public bool AddProductToCart(Guid userGuid, Guid shopGuid, Guid productGuid, int quantity)
        {
            return _serviceFacade.AddProductToCart(userGuid, shopGuid, productGuid, quantity);
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            return _serviceFacade.AddShopManager(userGuid, shopGuid, newManagaerGuid, priviliges);
            
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

        public bool EditProductInShop(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity, string name, string category)
        {
            return _serviceFacade.EditProductInShop(userGuid, shopGuid, productGuid, newPrice, newQuantity, name, category);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
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
            return _serviceFacade.OpenShop(userGuid);
        }

        public bool PurchaseBag(Guid userGuid)
        {
            return _serviceFacade.PurchaseBag(userGuid);
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

        public ICollection<Guid> SearchProduct(Guid userGuid, ICollection<string> toMatch, string searchType)
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
    }
}
