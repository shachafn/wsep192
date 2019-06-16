﻿using ApplicationCore.Data;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer;
using DomainLayer.Domains;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Entities.Users
{
    public class GuestUser : IUser
    {
        protected IUnitOfWork _unitOfWork;
        protected ShopDomain _shopDomain; 

        public Guid Guid { get; private set; }
        public bool IsAdmin => false;
        
        public GuestUser(Guid guid, IUnitOfWork unitOfWork, ShopDomain shopDomain)
        {
            Guid = guid;
            _unitOfWork = unitOfWork;
            _shopDomain = shopDomain;
        }

        public Guid OpenShop()
        {
            throw new BadStateException($"Tried to invoke OpenShop in GuestUser");
        }

        public Guid OpenShop(string shopName)
        {
            throw new BadStateException($"Tried to invoke OpenShop in GuestUser");
        }

        public void ReopenShop(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke ReopenShop in GuestUser");
        }

        public void CloseShop(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke CloseShop in GuestUser");
        }

        public void CloseShopPermanently(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke CloseShopPermanently in GuestUser");
        }

        public bool PurchaseCart(Guid shopGuid)
        {
            var cart = GetGuestCartAndCreateIfNeeded(shopGuid);

            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            //Can implement RollBack, purchase is given a Guid, shop.PurchaseCart returns a Guid,
            // if the user fails to pay later, we can delete the purchase and revert the shop quantities and cart content
            _shopDomain.ShoppingCartDomain.CheckDiscountPolicy(cart);
            if (!_shopDomain.PurchaseCart(shop, cart))
                return false;
            //External payment pay, if not true ---- rollback
            return true;
        }
        public double GetCartPrice(Guid shopGuid)
        {
            var cart = GetGuestCartAndCreateIfNeeded(shopGuid);
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            ShoppingCart tempCart = new ShoppingCart(cart.UserGuid, cart.ShopGuid);
            foreach (Tuple<ShopProduct, int> record in cart.PurchasedProducts)
            {
                tempCart.PurchasedProducts.Add(record);
            }


            _shopDomain.ShoppingCartDomain.CheckDiscountPolicy(tempCart);
            double totalPrice= _shopDomain.GetCartPrice(shop, tempCart);
            tempCart.UserGuid = Guid.Empty;
            tempCart.ShopGuid = Guid.Empty;
            tempCart.PurchasedProducts.Clear();
            return totalPrice;
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
            var cart = GetGuestCartAndCreateIfNeeded(shopGuid);
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            var actualProduct = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            cart.AddProductToCart(actualProduct, quantity);
            return true;
        }

        public bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in GuestUser");
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in GuestUser");
        }

        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var cart = GetGuestCartAndCreateIfNeeded(shopGuid);
            return cart.EditProductInCart(shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            var cart = GetGuestCartAndCreateIfNeeded(shopGuid);
            return cart.RemoveProductFromCart(shopProductGuid);
        }

        public ICollection<ShopProduct> GetAllProductsInCart(Guid shopGuid)
        {
            var cart = GetGuestCartAndCreateIfNeeded(shopGuid);
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

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            var searcher = new ProductsSearcher(searchType, _unitOfWork);
            return searcher.Search(toMatch);
        }

        private ShoppingCart GetGuestCartAndCreateIfNeeded(Guid shopGuid)
        {
            return DomainData.GuestsBagsCollection.GetShoppingBagAndCreateIfNeeded(Guid).GetShoppingCartAndCreateIfNeeded(shopGuid);
        }

        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewPurchasePolicy in GuestUser");
        }

        public Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewDiscountPolicy in GuestUser");
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory()
        {
            return _unitOfWork.ShopRepository.Query().SelectMany(shop => shop.GetPurchaseHistory(Guid)).ToList();
        }


    }
}
