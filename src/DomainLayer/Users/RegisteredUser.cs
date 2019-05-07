using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLaye.Users.States;
using DomainLayer.Extension_Methods;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Policies;
using DomainLayer.Users.States;

namespace DomainLayer.Users
{
    public class RegisteredUser : IUser
    {
        public Guid Guid { get => _baseUser.Guid; }
        public bool IsAdmin { get => _baseUser.IsAdmin; }
        private BaseUser _baseUser { get; set; }
        private IAbstractUserState State { get; set; }

        /// <summary>
        /// Default constructor, creates the user with a default Guest state.
        /// </summary>
        public RegisteredUser(BaseUser baseUser)
        {
            _baseUser = baseUser;
            var builder = new StateBuilder();
            State = builder.BuildState(BuyerUserState.BuyerUserStateString, this);
        }

        public bool SetState(IAbstractUserState newState)
        {
            State = newState;
            return true;
        }

        public ICollection<Guid> GetShoppingHistory() => State.GetShoppingHistory();

        public bool PurchaseCart(Guid shopGuid)
        {
            return State.PurchaseCart(_baseUser, shopGuid);
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

        public Guid AddProductToShop(Guid shopGuid, string name, string category, double price, int quantity)
        {
            return State.AddProductToShop(_baseUser, shopGuid, name, category, price, quantity);
        }

        public void EditProductInShop(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            State.EditProductInShop(_baseUser, shopGuid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid)
        {
            return State.RemoveProductFromShop(_baseUser, shopGuid, shopProductGuid);
        }

        public bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            return State.AddProductToCart(_baseUser, shopGuid, shopProductGuid, quantity);
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

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            return State.SearchProduct(toMatch, searchType);
        }
        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            return State.AddNewPurchasePolicy(userGuid, shopGuid, newPolicy);
        }

        public Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            return State.AddNewDiscountPolicy(userGuid, shopGuid, newPolicy);
        }

        public ICollection<Tuple<Guid, Product, int>> GetPurchaseHistory()
        {
            return DomainData.ShopsCollection.SelectMany(shop => shop.GetPurchaseHistory(Guid)).ToList();
        }
    }
}
