using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using DomainLayer.Extension_Methods;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;

namespace DomainLayer.Users.States
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

        public override bool AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            if (!shop.IsOwner(userGuid))
            {
                throw new IllegalOperationException("Tried to add new purchase policy to a shop that doesn't belong to him");
            }
            return shop.AddNewPurchasePolicy(newPolicy);
        }

        public override bool AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            if (!shop.IsOwner(userGuid))
            {
                throw new IllegalOperationException("Tried to add new discount policy to a shop that doesn't belong to him");
            }
            return shop.AddNewDiscountPolicy(newPolicy);
        }
    }
}
