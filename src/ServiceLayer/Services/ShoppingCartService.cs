﻿using DomainLayer;
using ServiceLayer.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ServiceLayer.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public bool AddProductToShoppingCart(Guid productGuid, Guid shopOfCartGuid, string username)
        {
            var user = User.GetUserByUsername(username);
            if (user == null) return false;
            var cart = user.currentBag.Carts.FirstOrDefault(c => c.Shop.Guid.Equals(shopOfCartGuid));
            if (cart == null) return false;
            var shop = Shop.GetShopByGuid(shopOfCartGuid);
            if (shop == null) return false;
            var product = shop.ShopProducts.FirstOrDefault(prod => prod.Product.Guid.Equals(productGuid));
            if (product == null) return false;

            cart.AddProduct(product);
            return true;
        }

        public bool ChangePurchasedProductAmount(string username, Guid shopOfCartGuid, Guid productGuid, int newAmount)
        {
            var cart = GetUserShoppingCartOfByShopGuid(username, shopOfCartGuid);
            cart.EditProduct(productGuid, newAmount);
            return true;
        }

        public IEnumerable<Guid> GetAllProducts(string username, Guid shopOfCartGuid)
        {
            var output = new List<Guid>();
            var cart = GetUserShoppingCartOfByShopGuid(username, shopOfCartGuid);
            return cart.ShopProducts.Select(prod => prod.Product.Guid);
        }

        public bool RemoveProduct(Guid productGuid, Guid shopOfCartGuid, string username)
        {
            var cart = GetUserShoppingCartOfByShopGuid(username, shopOfCartGuid);
            cart.RemoveProduct(productGuid);
            return true;
        }

        #region Helper Functions
        private ShoppingCart GetUserShoppingCartOfByShopGuid(string username, Guid shopOfCartGuid)
        {
            var user = User.GetUserByUsername(username);
            if (user == null) return null;
            var cart = user.currentBag.Carts.FirstOrDefault(c => c.Shop.Guid.Equals(shopOfCartGuid));
            if (cart == null) return null;
            return cart;
        }
        #endregion
    }
}
