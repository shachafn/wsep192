using System;
using System.Collections.Generic;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Data.Entitites.Users.States;

namespace DomainLayer.Data.Entitites
{
    public class User : IUser
    {
        private AbstractUserState State { get; set; }

        public Guid Guid { get => State.Guid; }
        public string Username { get => State.Username; }

        /// <summary>
        /// Default constructor, creates the user with a default Guest state.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public User(string username, string password)
        {
            State = new GuestUserState(username, password);
        }

        public void SetState(AbstractUserState newState)
        {
            State = newState;
        }

        public ICollection<ShoppingBag> GetShoppingHistory() => State.GetShoppingHistory();

        public void PurchaseBag()
        {
            State.PurchaseBag();
        }

        public Guid OpenShop() => State.OpenShop();        

        public bool CheckPass(string password) => State.CheckPass(password);

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
            return State.AddShopProduct(shopGuid, name, category, price, quantity);
        }

        public void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            State.EditShopProduct(shopGuid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid)
        {
            return State.RemoveShopProduct(shopGuid, shopProductGuid);
        }

        public bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            return State.AddProductToShoppingCart(shopGuid, shopProductGuid, quantity);
        }

        public bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            return State.AddShopOwner(shopGuid, newManagaerGuid);
        }

        public bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            return State.AddShopManager(shopGuid, newManagaerGuid, priviliges);
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return State.CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
        }

        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            return State.EditProductInCart(shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            return State.RemoveProductFromCart(shopGuid, shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid shopGuid)
        {
            return State.GetAllProductsInCart(shopGuid);
        }

        public bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            return State.RemoveShopManager(shopGuid, managerToRemoveGuid);
        }
    }
}
