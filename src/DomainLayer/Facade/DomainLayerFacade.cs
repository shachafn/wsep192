﻿using DomainLayer.Data;
using DomainLayer.Data.Entitites;
using DomainLayer.Domains;
using DomainLayer.Exceptions;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static DomainLayer.Data.Entitites.Shop;

namespace DomainLayer.Facade
{
    public class DomainLayerFacade : IDomainLayerFacade
    {
        UserDomain UserDomain = UserDomain.Instance;

        #region Singleton Implementation
        private static IDomainLayerFacade instance = null;
        private static readonly object padlock = new object();
        public static IDomainLayerFacade Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DomainLayerFacade();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");


        public bool Register(Guid userGuid, string username, string password)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, username, password);
            return UserDomain.Register(username, password);
        }

        public Guid Login(Guid userGuid, string username, string password)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, username, password);
            return UserDomain.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return UserDomain.LogoutUser(userGuid);
        }

        public Guid OpenShop(Guid userGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].OpenShop();
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid);
            // Need to actually pay for products
            // if success clear cart
            return DomainData.ShoppingBagsCollection[userGuid].ShoppingCarts.First(c => c.ShopGuid.Equals(shopGuid)).
                PurchaseCart(DomainData.LoggedInUsersEntityCollection[userGuid]); //Not implemented
        }

        public bool Initialize(Guid userGuid, string username, string password)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, username, password);

            // check for admin user exists - need to add IsAdmin to user
            // register if needed
            if (!External_Services.ExternalServicesManager._paymentSystem.IsAvailable())
                return false;
            if (!External_Services.ExternalServicesManager._supplySystem.IsAvailable())
                return false;

            throw new NotImplementedException();
        }

        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].ConnectToPaymentSystem();
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].ConnectToSupplySystem();
        }

        public Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, name, category, price, quantity);
            return DomainData.LoggedInUsersEntityCollection[userGuid].AddShopProduct(shopGuid, name, category, price, quantity);
        }

        public bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, productGuid, newPrice, newQuantity);
            DomainData.LoggedInUsersEntityCollection[userGuid].EditShopProduct(shopGuid, productGuid, newPrice, newQuantity);
            return true;
        }

        public bool RemoveShopProduct(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, shopProductGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].RemoveShopProduct(shopGuid, shopProductGuid);
        }

        public bool AddProductToShoppingCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopProductGuid, quantity);
            return DomainData.LoggedInUsersEntityCollection[userGuid].AddProductToShoppingCart(shopGuid, shopProductGuid, quantity);
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, newManagaerGuid, priviliges);
            return DomainData.LoggedInUsersEntityCollection[userGuid].AddShopManager(shopGuid, newManagaerGuid, priviliges);
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, ownerToRemoveGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, shopProductGuid, newAmount);
            return DomainData.LoggedInUsersEntityCollection[userGuid].EditProductInCart(shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, shopProductGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].RemoveProductFromCart(shopGuid, shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].GetAllProductsInCart(shopGuid);
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, userToRemoveGuid);
            //return user.RemoveUser(userToRemoveGuid); // not implemented
            throw new NotImplementedException();
        }

        public ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, productName);
            return DomainData.ShopsCollection[shopGuid].SearchProduct(productName);
        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            DomainLayerFacadeVerifier.VerifyMe(MethodBase.GetCurrentMethod(), userGuid, shopGuid, managerToRemoveGuid);
            return DomainData.LoggedInUsersEntityCollection[userGuid].RemoveShopManager(shopGuid, managerToRemoveGuid);
        }
    }
}
