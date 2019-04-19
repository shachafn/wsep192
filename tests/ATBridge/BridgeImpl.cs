using System;
using System.Collections.Generic;
using ServiceLayer;
using DomainLayer.Data.Entitites;

namespace ATBridge
{
    public class BridgeImpl : IBridge
    {
        private readonly ServiceFacade _serviceFacade;

        public BridgeImpl()
        {
            _serviceFacade = new ServiceFacade();
        }

        public bool AddProductToShoppingCart(Guid userGuid, Guid productGuid, Guid shopGuid, int quantity)
        {
            return _serviceFacade.AddProductToShoppingCart(userGuid, productGuid, shopGuid, quantity);
        }

        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            return _serviceFacade.AddShopManager(userGuid, shopGuid, newManagaerGuid, priviliges);
            //throw new NotImplementedException();
        }

        public bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid)
        {
            return _serviceFacade.AddShopOwner(userGuid, shopGuid, newShopOwnerGuid);
            //throw new NotImplementedException();
        }

        public Guid AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            return _serviceFacade.AddShopProduct(userGuid, shopGuid, name, category, price, quantity);
            //throw new NotImplementedException();
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return _serviceFacade.CascadeRemoveShopOwner(userGuid, shopGuid, ownerToRemoveGuid);
            //throw new NotImplementedException();
        }

        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            return _serviceFacade.ConnectToSupplySystem(userGuid);
           // throw new NotImplementedException();
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            return _serviceFacade.ConnectToSupplySystem(userGuid);
            //throw new NotImplementedException();
        }

        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return _serviceFacade.EditProductInCart(userGuid, shopGuid, shopProductGuid, newAmount);
            //throw new NotImplementedException();
        }

        public bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            //return _serviceFacade.EditProductInCart(userGuid, shopGuid, productGuid, newPrice, newQuantity);
            throw new NotImplementedException();
        }

        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            return _serviceFacade.GetAllProductsInCart(userGuid, shopGuid);
            //throw new NotImplementedException();
        }

        public bool Initialize(Guid userGuid, string username, string password)
        {
            return _serviceFacade.Initialize(userGuid, username, password);
        }

        public Guid Login(Guid userGuid, string username, string password)
        {
            return _serviceFacade.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            return _serviceFacade.Logout(userGuid);
            //throw new NotImplementedException();
        }

        public Guid OpenShop(Guid userGuid)
        {
            return _serviceFacade.OpenShop(userGuid);
            //throw new NotImplementedException();
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            //return _serviceFacade.PurchaseBag(userGuid, shopGuid);
            throw new NotImplementedException();
        }

        public bool Register(Guid userGuid, string username, string password)
        {
            return _serviceFacade.Register(userGuid, username, password);
        }

        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            return _serviceFacade.RemoveProductFromCart(userGuid, shopGuid, shopProductGuid);
            //throw new NotImplementedException();
        }

        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return _serviceFacade.RemoveShopManager(userGuid, shopGuid, managerToRemoveGuid);
            //throw new NotImplementedException();
        }

        public bool RemoveShopProduct(Guid userGuid, Guid shopProductGuid, Guid shopGuid)
        {
            //return _serviceFacade.RemoveShopProduct(userGuid, shopProductGuid, shopGuid);
            throw new NotImplementedException();
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            return _serviceFacade.RemoveUser(userGuid, userToRemoveGuid);
            //throw new NotImplementedException();
        }

        public ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            return _serviceFacade.SearchProduct(userGuid, shopGuid, productName);
            //throw new NotImplementedException();
        }
    }
}
