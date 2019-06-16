using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Policies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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

        public void CheckDiscountPolicy(ShoppingCart cart)
        {
            Shop shop = _unitOfWork.ShopRepository.FindByIdOrNull(cart.ShopGuid);
            BaseUser user = _unitOfWork.BaseUserRepository.FindByIdOrNull(cart.UserGuid);

            //Copy the list so you can iterate and add the discount to it
            ICollection<Tuple<ShopProduct, int>> tempPurchasedProducts = new List<Tuple<ShopProduct, int>>();
            foreach (Tuple<ShopProduct, int> record in cart.PurchasedProducts)
            {
                tempPurchasedProducts.Add(record);
            }

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
                    if (discountProductAndQuantity != null && !alreadyAddedDiscount) { 
                        cart.AddProductToCart(discountProductAndQuantity.Item1, discountProductAndQuantity.Item2);
                        alreadyAddedDiscount = true;
                    }
                }
            }
            

        }
    }
}
