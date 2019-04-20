using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class BuyerUserState : AbstractUserState
    {
        public const string BuyerUserStateString = "BuyerUserState";

        public ICollection<ShoppingBag> PurchaseHistory { get; set; }
        public ShoppingBag CurrentBag { get; set; }


        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            return PurchaseHistory;
        }

        public override bool PurchaseBag()
        {
            if (!CurrentBag.Empty())
            {
                CurrentBag.PurchaseBag(this);//sending the user itself as the buyer
                PurchaseHistory.Add(CurrentBag);
                return true;
            }
            return false;
        }

        public override Guid OpenShop(BaseUser baseUser)
        {
            throw new BadStateException($"Tried to invoke OpenShop in Buyer State");
        }

        public override bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in Buyer State");
        }

        public override bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in Buyer State");
        }

        public override bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in Buyer State");
        }

        public override Guid AddProductToShop(BaseUser baseUser, Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShop in Buyer State");
        }

        public override void EditProductInShop(BaseUser baseUser, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Buyer State");
        }

        public override bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopProduct in Buyer State");
        }

        public override bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var cart = GetCartAndCreateIfNeeded(baseUser, shopGuid);
            cart.AddProductToCart(shopProductGuid, quantity);
            return true;
        }

        public override bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in Buyer State");
        }

        public override bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in Buyer State");
        }

        public override bool EditProductInCart(BaseUser baseuser, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var cart = GetCartAndCreateIfNeeded(baseuser, shopGuid);
            return cart.EditProductInCart(shopProductGuid, newAmount);
        }

        public override bool RemoveProductFromCart(BaseUser baseuser, Guid shopGuid, Guid shopProductGuid)
        {
            var cart = GetCartAndCreateIfNeeded(baseuser, shopGuid);
            return cart.RemoveProductFromCart(shopProductGuid);
        }

        public override ICollection<Guid> GetAllProductsInCart(BaseUser baseuser, Guid shopGuid)
        {
            var cart = GetCartAndCreateIfNeeded(baseuser, shopGuid);
            return cart.GetAllProductsInCart();
        }

        public override bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in Buyer State");
        }

        public override bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Buyer State");
        }

        private ShoppingCart GetCartAndCreateIfNeeded(BaseUser baseUser, Guid shopGuid)
        {
            var userBag = DomainData.ShoppingBagsCollection.FirstOrDefault(bag => bag.UserGuid.Equals(baseUser.Guid));
            if (userBag == null)
            {
                userBag = new ShoppingBag(baseUser.Guid);
                DomainData.ShoppingBagsCollection.Add(baseUser.Guid, userBag);
            }
            var userCart = userBag.ShoppingCarts.FirstOrDefault(cart => cart.ShopGuid.Equals(shopGuid));
            if (userCart == null)
            {
                userCart = new ShoppingCart(baseUser.Guid, shopGuid);
                userBag.ShoppingCarts.Add(userCart);
            }
            return userCart;
        }
    }
}
