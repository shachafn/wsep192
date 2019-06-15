﻿using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Policies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Domains
{
    public class ShoppingCartDomain
    {
        protected IUnitOfWork _unitOfWork;
        protected ILogger<ShoppingCartDomain> _logger;

        public ShoppingCartDomain(IUnitOfWork unitOfWork, ILogger<ShoppingCartDomain> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void CheckDiscountPolicy(ShoppingBag bag, Guid shopGuid)
        {
            CheckDiscountPolicyWithoutUpdate(bag, shopGuid);

            _unitOfWork.BagRepository.Update(bag);
        }
        public void CheckDiscountPolicyWithoutUpdate(ShoppingBag bag, Guid shopGuid)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid);
            Shop shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            BaseUser user = _unitOfWork.BaseUserRepository.FindByIdOrNull(bag.UserGuid);

            //Copy the list so you can iterate and add the discount to it
            ICollection<Tuple<ShopProduct, int>> tempPurchasedProducts = new List<Tuple<ShopProduct, int>>();
            foreach (Tuple<ShopProduct, int> record in cart.PurchasedProducts)
            {
                tempPurchasedProducts.Add(record);
            }


            //foreach (Tuple<ShopProduct, int> record in tempPurchasedProducts)
            //{
            //    foreach (IDiscountPolicy policy in shop.DiscountPolicies)
            //    {
            //        var discountProductAndQuantity = policy.ApplyPolicy(cart, record.Item1.Guid, record.Item2, user, _unitOfWork);
            //        if (discountProductAndQuantity != null)
            //            cart.AddProductToCart(discountProductAndQuantity.Item1, discountProductAndQuantity.Item2);
            //    }
            //}
            //Copy the list so you can iterate and add the discount to it


            //First Apply CartDiscountPolicy
            //foreach (IDiscountPolicy policy in shop.DiscountPolicies)
            //{
            //    if (policy.GetType() != typeof(CartDiscountPolicy)) continue;
            //    var discountProductAndQuantity = policy.ApplyPolicy(cart, Guid.Empty, 0, user, _unitOfWork);
            //    if (discountProductAndQuantity != null /*&& discountProductAndQuantity.Item1.Product.Name.Equals("Discount - cart")*/)
            //        cart.AddProductToCart(discountProductAndQuantity.Item1, discountProductAndQuantity.Item2);
            //}


            foreach (IDiscountPolicy policy in shop.DiscountPolicies)
            {
                bool alreadyAddedDiscount = false;
                foreach (Tuple<ShopProduct, int> record in tempPurchasedProducts)
                {
                    //if (policy.GetType() == typeof(CartDiscountPolicy)) continue;
                    var discountProductAndQuantity = policy.ApplyPolicy(cart, record.Item1.Guid, record.Item2, user, _unitOfWork);
                    if (discountProductAndQuantity != null && !alreadyAddedDiscount)
                    {
                        cart.AddProductToCart(discountProductAndQuantity.Item1, discountProductAndQuantity.Item2);
                        alreadyAddedDiscount = true;
                    }
                }
            }

        }
        public void ClearAllDiscounts(ShoppingBag bag, Guid shopGuid)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid);
            var tmp= cart.PurchasedProducts.Where(sp => sp.Item1.Price > 0);
            cart.PurchasedProducts.Clear();
            foreach(Tuple<ShopProduct,int> sp in tmp)
            {
                cart.PurchasedProducts.Add(sp);
            }
        }
            public void AddProductToCart(ShoppingBag bag, Guid shopGuid, ShopProduct actualProduct, int quantity)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid);
            cart.AddProductToCart(actualProduct, quantity);
            _unitOfWork.BagRepository.Update(bag);
        }

        public bool EditProductInCart(ShoppingBag bag, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid);
            var result = cart.EditProductInCart(shopProductGuid, newAmount);
            _unitOfWork.BagRepository.Update(bag);
            return result;
        }

        public bool RemoveProductFromCart(ShoppingBag bag, Guid shopGuid, Guid shopProductGuid)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid);
            var result = cart.RemoveProductFromCart(shopProductGuid);
            if (cart.PurchasedProducts.Count == 0)
                bag.ShoppingCarts.Remove(cart);
            _unitOfWork.BagRepository.Update(bag);
            return result;
        }

        public void PurchaseCart(ShoppingBag bag, Guid shopGuid)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid);
            cart.PurchaseCart();
            bag.ShoppingCarts.Remove(cart);
            _unitOfWork.BagRepository.Update(bag);
        }
    }
}
