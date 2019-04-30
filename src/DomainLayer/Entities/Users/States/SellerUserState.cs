using ApplicationCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class SellerUserState : AbstractUserState
    {
        public const string SellerUserStateString = "SellerUserState";


        public ICollection<Shop> ShopsOwned { get; set; }

        public override ICollection<Guid> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Seller State");
        }

        public override Guid OpenShop(BaseUser baseUser)
        {
            var shop = new Shop(baseUser.Guid);
            ShopsOwned.Add(shop);
            DomainData.ShopsCollection.Add(shop.Guid, shop);
            return shop.Guid;
        }

        public override bool PurchaseBag()
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

        public override Guid AddProductToShop(BaseUser baseUser, Guid shopGuid, 
            string name, string category, double price, int quantity)
        {
            return DomainData.ShopsCollection[shopGuid].AddProductToShop(baseUser.Guid, new Product(name, category), price, quantity);
        }

        public override void EditProductInShop(BaseUser baseUser,Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            DomainData.ShopsCollection[shopGuid].EditProductInShop(baseUser.Guid, productGuid, newPrice, newQuantity);
        }

        public override bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            DomainData.ShopsCollection[shopGuid].RemoveProductFromShop(baseUser.Guid, shopProductGuid);
            return true;
        }

        public override bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToCart in Seller State");
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

        public override bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newOwnerGuid)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            return shop.AddOwner(baseUser.Guid, newOwnerGuid);
        }
        public override ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            throw new BadStateException($"Tried to invoke SearchProduct in Seller State");
        }

        public override bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke PurchaseCart in Seller State");
        }
    }
}
