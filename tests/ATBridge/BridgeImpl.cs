using System;
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

        public bool AddProductToShoppingCart(Guid userGuid, Guid productGuid, Guid shopGuid, int quantity)
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

        public Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new NotImplementedException();
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new NotImplementedException();
        }

        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new NotImplementedException();
        }

        public bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            throw new NotImplementedException();
        }

        public bool Initialize(Guid userGuid, string username, string password)
        {
            throw new NotImplementedException();
        }

        public Guid Login(Guid userGuid, string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Logout(Guid userGuid)
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

        public bool Register(Guid userGuid, string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            throw new NotImplementedException();
        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new NotImplementedException();
        }

        public bool RemoveShopProduct(Guid userGuid, Guid shopProductGuid, Guid shopGuid)
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
