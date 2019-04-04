using DomainLayer.Exceptions;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class SellerUserState : AbstractUserState
    {
        public SellerUserState(string username, string password) : base (username, password) { }
        public ICollection<Shop> ShopsOwned { get; set; }

        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Seller State");
        }

        public override Guid OpenShop()
        {
            var shop = new Shop(Guid);
            ShopsOwned.Add(shop);
            return shop.Guid;
        }

        public override void PurchaseBag()
        {
            throw new BadStateException($"Tried to invoke PurchaseBag in Seller State");
        }

        public override bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in Seller State");
        }

        public override bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in Seller State");
        }

        public override bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in Seller State");
        }

        public override Guid AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            return DomainData.ShopsCollection[shopGuid].AddProduct(Guid, new Product(name, category), price, quantity);
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            DomainData.ShopsCollection[shopGuid].EditProduct(Guid, productGuid, newPrice, newQuantity);
        }

        public override bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid)
        {
            DomainData.ShopsCollection[shopGuid].RemoveProduct(Guid, shopProductGuid);
            return true;
        }

        public override bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShoppingCart in Seller State");
        }

        public override bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            DomainData.ShopsCollection[shopGuid].AddShopManager(Guid, newManagaerGuid, priviliges);
            return true;
        }


        public override bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return DomainData.ShopsCollection[shopGuid].CascadeRemoveShopOwner(Guid, ownerToRemoveGuid);
        }

        public override bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Seller State");
        }

        public override bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Seller State");
        }

        public override ICollection<Guid> GetAllProductsInCart(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Seller State");
        }

        public override bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            return DomainData.ShopsCollection[shopGuid].RemoveShopManager(Guid, managerToRemoveGuid);
        }

        public override bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            DomainData.ShopsCollection[shopGuid].AddShopOwner(Guid, newManagaerGuid);
            return true;
        }
    }
}
