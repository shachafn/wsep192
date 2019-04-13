using DomainLayer.Exceptions;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class SellerUserState : AbstractUserState
    {
        public ICollection<Shop> ShopsOwned { get; set; }

        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Seller State");
        }

        public override Guid OpenShop(BaseUser baseUser)
        {
            var shop = new Shop(baseUser.Guid);
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

        public override Guid AddShopProduct(BaseUser baseUser, Guid shopGuid, 
            string name, string category, double price, int quantity)
        {
            return DomainData.ShopsCollection[shopGuid].AddProduct(baseUser.Guid, new Product(name, category), price, quantity);
        }

        public override void EditShopProduct(BaseUser baseUser,Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            DomainData.ShopsCollection[shopGuid].EditProduct(baseUser.Guid, productGuid, newPrice, newQuantity);
        }

        public override bool RemoveShopProduct(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            DomainData.ShopsCollection[shopGuid].RemoveProduct(baseUser.Guid, shopProductGuid);
            return true;
        }

        public override bool AddProductToShoppingCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShoppingCart in Seller State");
        }

        public override bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            DomainData.ShopsCollection[shopGuid].AddShopManager(baseUser.Guid, newManagaerGuid, priviliges);
            return true;
        }

        public override bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return DomainData.ShopsCollection[shopGuid].CascadeRemoveShopOwner(baseUser.Guid, ownerToRemoveGuid);
        }

        public override bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Seller State");
        }

        public override bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Seller State");
        }

        public override ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Seller State");
        }

        public override bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return DomainData.ShopsCollection[shopGuid].RemoveShopManager(baseUser.Guid, managerToRemoveGuid);
        }

        public override bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid)
        {
            DomainData.ShopsCollection[shopGuid].AddShopOwner(baseUser.Guid, newManagaerGuid);
            return true;
        }
    }
}
