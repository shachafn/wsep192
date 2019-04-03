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

        public Guid Login(Guid userGuid, string username, string password)
        {
            return _domainLayerFacade.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            return _domainLayerFacade.Logout(userGuid);
        }

        public bool Register(Guid userGuid, string username, string password)
        {
            return _domainLayerFacade.Register(username, password);
        }

        public bool Initialize(Guid userGuid, string username = null, string password = null)
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

        public Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            return _domainLayerFacade.AddShopProduct(userGuid, shopGuid, name, category, price, quantity);
        }

        public bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _domainLayerFacade.EditShopProduct(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveShopProduct(Guid userGuid, Guid shopProductGuid, Guid shopGuid)
        {
            throw new NotImplementedException();
        }

        public bool AddProductToShoppingCart(Guid userGuid, Guid productGuid, Guid shopOfCartGuid)
        {
            throw new NotImplementedException();
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new NotImplementedException();
        }

        public bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid)
        {
            throw new NotImplementedException();
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new NotImplementedException();
        }

        public bool EditProductInCart(Guid userGuid, Guid shopOfCartGuid, Guid shopProductGuid, int newAmount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetAllProductsInCart(Guid userGuid, Guid shopOfCartGuid)
        {
            throw new NotImplementedException();
        }

        public Guid OpenShop(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopProductGuid, Guid shopOfCartGuid)
        {
            throw new NotImplementedException();
        }


        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            throw new NotImplementedException();
        }

        public ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            throw new NotImplementedException();
        }
    }
}
