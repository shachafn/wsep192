﻿using DomainLayer.Data.Entitites;
using DomainLayer.ExposedClasses;
using DomainLayer.Facade;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    public class ServiceFacade 
    {
        IDomainLayerFacade _domainLayerFacade = DomainLayerFacade.Instance;

        #region Singleton Implementation
        private static ServiceFacade instance = null;
        private static readonly object padlock = new object();
        public static ServiceFacade Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceFacade();
                    }
                    return instance;
                }
            }
        }
        #endregion

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

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity, string name, string category)
        {
            return _domainLayerFacade.EditProductInShop(userIdentifier, shopGuid, productGuid, newPrice, newQuantity, name, category);
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

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.OpenShop(userIdentifier);
        }

        public bool PurchaseBag(UserIdentifier userIdentifier)
        {
            return _domainLayerFacade.PurchaseBag(userIdentifier);
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            return _domainLayerFacade.RemoveUser(userIdentifier, userToRemoveGuid);
        }

        public ICollection<Guid> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
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
    }
}
