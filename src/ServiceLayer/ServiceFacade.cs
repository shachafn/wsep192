using DomainLayer.Data.Entitites;
using DomainLayer.Facade;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    public class ServiceFacade : IServiceFacade
    {
        IDomainLayerFacade _domainLayerFacade = DomainLayerFacade.Instance;

        #region Singleton Implementation
        private static IServiceFacade instance = null;
        private static readonly object padlock = new object();
        public static IServiceFacade Instance
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
        public bool Register(Guid userGuid, string username, string password)
        {
            return _domainLayerFacade.Register(userGuid, username, password);
        }

        public Guid Login(Guid userGuid, string username, string password)
        {
            return _domainLayerFacade.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            return _domainLayerFacade.Logout(userGuid);
        }
        #endregion

        #region Admin
        public Guid Initialize(Guid userGuid, string username, string password)
        {
            return _domainLayerFacade.Initialize(userGuid, username, password);
        }
        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            return _domainLayerFacade.ConnectToPaymentSystem(userGuid);
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            return _domainLayerFacade.ConnectToSupplySystem(userGuid);
        }
        #endregion

        #region Shop Products

        public Guid AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            return _domainLayerFacade.AddProductToShop(userGuid, shopGuid, name, category, price, quantity);
        }

        public bool EditProductInShop(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _domainLayerFacade.EditProductInShop(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveProductFromShop(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            return _domainLayerFacade.RemoveProductFromShop(userGuid, shopGuid, shopProductGuid);
        }
        #endregion

        #region Shop Owners and Managers
        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            return _domainLayerFacade.AddShopManager(userGuid, shopGuid, newManagaerGuid, priviliges);
        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return _domainLayerFacade.RemoveShopManager(userGuid, shopGuid, managerToRemoveGuid);
        }

        public bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid)
        {
            //Creating an owner with null priviliges is an owner (An manager is actually an owner with restricted priviliges)
            return _domainLayerFacade.AddShopManager(userGuid, shopGuid, newShopOwnerGuid, null);
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return _domainLayerFacade.CascadeRemoveShopOwner(userGuid, shopGuid, ownerToRemoveGuid);
        }
        #endregion

        #region Shopping Cart
        public bool AddProductToCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            return _domainLayerFacade.AddProductToCart(userGuid, shopGuid, shopProductGuid, quantity);
        }


        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            return _domainLayerFacade.RemoveProductFromCart(userGuid, shopGuid, shopProductGuid);
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return _domainLayerFacade.EditProductInCart(userGuid, shopGuid, shopProductGuid, newAmount);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            return _domainLayerFacade.GetAllProductsInCart(userGuid, shopGuid);
        }
        #endregion

        public Guid OpenShop(Guid userGuid)
        {
            return _domainLayerFacade.OpenShop(userGuid);
        }

        public bool PurchaseBag(Guid userGuid)
        {
            return _domainLayerFacade.PurchaseBag(userGuid);
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            return _domainLayerFacade.RemoveUser(userGuid, userToRemoveGuid);
        }

        public ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            return _domainLayerFacade.SearchProduct(userGuid, shopGuid, productName);
        }

        public bool ChangeUserState(Guid userGuid, string newState)
        {
            return _domainLayerFacade.ChangeUserState(userGuid, newState);
        }
    }
}
