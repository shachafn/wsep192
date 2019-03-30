using DomainLayer;
using ServiceLayer.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Services
{
    public class ShopService : IShopService
    {
        public Guid AddProductToShop(string name, string category, double price, int quantity, Guid shopGuid)
        {
            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop == null) return Guid.Empty;
            var product = new Product(name, category);
            shop.AddProduct(product, price, quantity);
            return product.ProductGuid;
        }

        public bool RemoveProductFromShop(Guid productGuid, Guid shopGuid)
        {
            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop == null) return false;
            shop.RemoveProduct(productGuid);
            return true;
        }

        public bool AddShopManager(Guid shopGuid, string ownerUsername, string managerToAddUsername, List<string> priviliges)
        {
            var managerToAdd = User.GetUserByUsername(ownerUsername);
            if (managerToAdd == null) return false;

            var owner = User.GetUserByUsername(ownerUsername);
            if (owner == null) return false;

            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop == null) return false;

            var shopOwner = ShopOwner.GetShopOwner(owner, shop);
            if (shopOwner == null) return false;

            return shopOwner.AddManager(managerToAdd, new ShopOwner.ManagingPrivileges(priviliges));
        }

        public bool AddShopOwner(Guid shopGuid, string ownerUsername, string managerToAddUsername)
        {
            var managerToAdd = User.GetUserByUsername(ownerUsername);
            if (managerToAdd == null) return false;

            var owner = User.GetUserByUsername(ownerUsername);
            if (owner == null) return false;

            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop == null) return false;

            var shopOwner = ShopOwner.GetShopOwner(owner, shop);
            if (shopOwner == null) return false;

            return shopOwner.AddOwner(managerToAdd);
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, string ownerUsername)
        {
            var owner = User.GetUserByUsername(ownerUsername);
            if (owner == null) return false;

            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop == null) return false;

            var shopOwner = ShopOwner.GetShopOwner(owner, shop);
            if (shopOwner == null) return false;

            return shopOwner.RemoveOwner(shopOwner);
        }

        public bool EditProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop == null) return false;
            shop.EditProduct(productGuid, newPrice, newQuantity);
            return true;
        }

        public bool RemoveShopManager(Guid shopGuid, string ownerUsername)
        {
            return CascadeRemoveShopOwner(shopGuid, ownerUsername);
        }

        public IEnumerable<Product> SearchProduct(Guid shopGuid, string productName)
        {
            var output = new List<Product>();
            var shop = Shop.GetShopByGuid(shopGuid);
            if (shop != null)
                return shop.SearchProducts(productName);
            return output;
        }
    }
}
