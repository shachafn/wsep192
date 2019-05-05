using ApplicationCore.Data;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using DomainLayer;
using DomainLayer.Extension_Methods;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Entities.Users
{
    public class GuestUser : IUser
    {
        public Guid Guid { get; private set; }
        public bool IsAdmin => false;
        public ShoppingBag CurrentBag { get; private set; }

        public GuestUser(Guid guid)
        {
            Guid = guid;
        }

        public ICollection<Guid> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in GuestUser");
        }

        public Guid OpenShop()
        {
            throw new BadStateException($"Tried to invoke OpenShop in GuestUser");
        }

        public bool PurchaseBag()
        {
            if (!CurrentBag.IsEmpty())
            {
                //CurrentBag.PurchaseBag();
                return true;
            }
            return false;
        }

        public bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in GuestUser");
        }

        public bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in GuestUser");
        }

        public bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in GuestUser");
        }

        public Guid AddProductToShop(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShop in GuestUser");
        }

        public void EditProductInShop( Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditProductInShop in GuestUser");
        }

        public bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromShop in GuestUser");
        }

        public bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var cart = GetCartAndCreateIfNeeded(Guid, shopGuid);
            cart.AddProductToCart(shopProductGuid, quantity);
            return true;
        }

        public bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in GuestUser");
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in GuestUser");
        }

        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var cart = GetCartAndCreateIfNeeded(Guid, shopGuid);
            return cart.EditProductInCart(shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            var cart = GetCartAndCreateIfNeeded(Guid, shopGuid);
            return cart.RemoveProductFromCart(shopProductGuid);
        }

        public ICollection<Guid> GetAllProductsInCart(Guid shopGuid)
        {
            var cart = GetCartAndCreateIfNeeded(Guid, shopGuid);
            return cart.GetAllProductsInCart();
        }

        public bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in GuestUser");
        }

        public bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in GuestUser");
        }

        public ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            var searcher = new ProductsSearcher(searchType);
            return searcher.Search(toMatch);
        }

        private ShoppingCart GetCartAndCreateIfNeeded(Guid userGuid, Guid shopGuid)
        {
            ShoppingBag bag = null;
            if (CurrentBag == null)
            {
                if (!DomainData.ShoppingBagsCollection.ContainsKey(userGuid))
                {
                    bag = new ShoppingBag(userGuid);
                    DomainData.ShoppingBagsCollection.Add(userGuid, bag);
                }
            }
            CurrentBag = bag;
            ShoppingCart cart = null;
            if (!bag.ShoppingCarts.Any(c => c.ShopGuid.Equals(shopGuid)))
            {
                cart = new ShoppingCart(userGuid, shopGuid);
                bag.ShoppingCarts.Add(cart);
            }
            return cart;
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            ShoppingCart cart = GetCartAndCreateIfNeeded(userGuid, shopGuid);
            cart.PurchaseCart();
            return true;
        }
        public bool SetState(IAbstractUserState newState)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in GuestUser");
        }

        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewPurchasePolicy in GuestUser");
        }

        public bool AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewDiscountPolicy in GuestUser");
        }
    }
}
