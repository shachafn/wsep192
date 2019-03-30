using DomainLayer;
using ServiceLayer.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Services
{
    class ShopService : IShopService
    {
        public bool AddProductToShop(Product product, DomainLayer.Shop shop, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool AddShopManager(string username, Shop shop, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool AddShopOwner(string username, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool CascadeRemoveShopOwner(string username, Shop shop, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool EditProduct(Product product, double newPrice, int newQuantity, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromShop(Product product, DomainLayer.Shop shop, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool RemoveShopManager(string username, Shop shop, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> SearchProduct(string productName)
        {
            throw new NotImplementedException();
        }
    }
}
