using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using DomainLayer.Extension_Methods;
using DomainLayer.Users.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class BuyerUserState : AbstractUserState
    {
        public const string BuyerUserStateString = "BuyerUserState";

        public ICollection<Guid> PurchaseHistory { get; set; }
        public ShoppingBag CurrentBag { get; set; }


        public override ICollection<Guid> GetShoppingHistory()
        {
            return PurchaseHistory;
        }

        public override bool PurchaseBag()
        {
            if (!CurrentBag.IsEmpty())
            {
                //CurrentBag.PurchaseBag();
                foreach (var cart in CurrentBag.ShoppingCarts)
                    foreach (var p in cart.PurchasedProducts)
                        PurchaseHistory.Add(p.Item1);

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
            throw new BadStateException($"Tried to invoke EditProductInShop in Buyer State");
        }

        public override bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromShop in Buyer State");
        }

        public override bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var cart = GetCartAndCreateIfNeeded(baseUser.Guid, shopGuid);
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
            var cart = GetCartAndCreateIfNeeded(baseuser.Guid, shopGuid);
            return cart.EditProductInCart(shopProductGuid, newAmount);
        }

        public override bool RemoveProductFromCart(BaseUser baseuser, Guid shopGuid, Guid shopProductGuid)
        {
            var cart = GetCartAndCreateIfNeeded(baseuser.Guid, shopGuid);
            return cart.RemoveProductFromCart(shopProductGuid);
        }

        public override ICollection<Guid> GetAllProductsInCart(BaseUser baseuser, Guid shopGuid)
        {
            var cart = GetCartAndCreateIfNeeded(baseuser.Guid, shopGuid);
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

        public override ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            var searcher = new ProductsSearcher(searchType);
            return searcher.Search(toMatch);
        }

        private ShoppingCart GetCartAndCreateIfNeeded(Guid userGuid, Guid shopGuid)
        {
            if (CurrentBag == null)
            {
                if (!DomainData.ShoppingBagsCollection.ContainsKey(userGuid))
                {
                    CurrentBag = new ShoppingBag(userGuid);
                    DomainData.ShoppingBagsCollection.Add(userGuid, CurrentBag);
                }
                else
                    CurrentBag = DomainData.ShoppingBagsCollection[userGuid];
            }
            ShoppingCart cart = null;
            if (!CurrentBag.ShoppingCarts.Any(c => c.ShopGuid.Equals(shopGuid)))
            {
                cart = new ShoppingCart(userGuid, shopGuid);
                CurrentBag.ShoppingCarts.Add(cart);
            }
            return CurrentBag.ShoppingCarts.First(c => c.ShopGuid.Equals(shopGuid));
        }

        public override bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            ShoppingCart cart = GetCartAndCreateIfNeeded(userGuid, shopGuid);
            //Checking discount policy. Pay attention that discount policies are not supposed to be exist, and changes
            //on the cart may be occured.
            ShoppingCart.CheckDiscountPolicy(ref cart);
            cart.PurchaseCart(); //If there are problems with money exception should be handled.
            //TODO: Get user and take money from his account
            return true;
        }
    }
}
