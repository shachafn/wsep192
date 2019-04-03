using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class SellerUserState : AbstractUserState
    {
        public SellerUserState(string username, string password) : base (username, password) { }
        public ICollection<Shop> ShopsOwned { get; set; }

        public override bool AddShopOwner(Guid shopGuid, Guid userGuid)
        {
            var shop = ShopsOwned.FirstOrDefault(s => s.Guid.Equals(shopGuid));
            if (shop == null) throw new ShopNotFoundException($"No shop found with Guid");
            var owner = shop.GetOwner(userGuid);
            return owner.AddOwner(userGuid);
        }

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

        public override void AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            if (!DomainData.ShopsCollection.ContainsKey(shopGuid))
                throw new ShopNotFoundException($"No shop found with Guid {shopGuid}.");

            var shop = DomainData.ShopsCollection[shopGuid];
            shop.AddProduct(Guid, new Product(name, category), price, quantity);
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            if (!DomainData.ShopsCollection.ContainsKey(shopGuid))
                throw new ShopNotFoundException($"No shop found with Guid {shopGuid}.");

            var shop = DomainData.ShopsCollection[shopGuid];
            shop.EditProduct(Guid, productGuid, newPrice, newQuantity);
        }
    }
}
