using System;
using System.Collections.Generic;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Data.Entitites.Users.States;

namespace DomainLayer.Data.Entitites
{
    public class User
    {
        public Guid Guid { get => _baseUser.Guid; }
        private BaseUser _baseUser { get; set; }
        private AbstractUserState State { get; set; }

        /// <summary>
        /// Default constructor, creates the user with a default Guest state.
        /// </summary>
        public User(BaseUser baseUser)
        {
            var builder = new StateBuilder();
            State = builder.BuildState("BuyerUserState", this);
        }

        public bool SetState(AbstractUserState newState)
        {
            //if (newState is AdminUserState && !BaseUser.IsAdmin) CHECK ADMIN
            State = newState;
            return true;
        }

        public ICollection<ShoppingBag> GetShoppingHistory() => State.GetShoppingHistory();

        public void PurchaseBag()
        {
            State.PurchaseBag();
        }

        public Guid OpenShop() => State.OpenShop(_baseUser);        

        public bool RemoveUser(Guid userToRemoveGuid)
        {
            return State.RemoveUser(userToRemoveGuid);
        }

        public bool ConnectToPaymentSystem()
        {
            return State.ConnectToPaymentSystem();
        }
        public bool ConnectToSupplySystem()
        {
            return State.ConnectToSupplySystem();
        }

        public Guid AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            return State.AddShopProduct(_baseUser, shopGuid, name, category, price, quantity);
        }

        public void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            State.EditShopProduct(_baseUser, shopGuid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid)
        {
            return State.RemoveShopProduct(_baseUser, shopGuid, shopProductGuid);
        }

        public bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            return State.AddProductToShoppingCart(_baseUser, shopGuid, shopProductGuid, quantity);
        }

        public bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            return State.AddShopOwner(_baseUser, shopGuid, newManagaerGuid);
        }

        public bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            return State.AddShopManager(_baseUser, shopGuid, newManagaerGuid, priviliges);
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return State.CascadeRemoveShopOwner(_baseUser, shopGuid, ownerToRemoveGuid);
        }

        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return State.EditProductInCart(_baseUser, shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            return State.RemoveProductFromCart(_baseUser, shopGuid, shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid shopGuid)
        {
            return State.GetAllProductsInCart(_baseUser, shopGuid);
        }

        public bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            return State.RemoveShopManager(_baseUser, shopGuid, managerToRemoveGuid);
        }
    }
}
