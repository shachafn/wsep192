using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;

namespace ATBridge
{
    class BridgeImpl : IBridge
    {
        public bool AddProduct()
        {
            throw new NotImplementedException();
        }

        public bool AddProduct(ShopProduct product)
        {
            throw new NotImplementedException();
        }

        public bool AddReview(User user, string text)
        {
            throw new NotImplementedException();
        }

        public bool EditProduct(Product product, double price, int quantity)
        {
            throw new NotImplementedException();
        }

        public void EditProfile()
        {
            throw new NotImplementedException();
        }

        public void GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public List<ShoppingBag> GetPurchaseHistory()
        {
            throw new NotImplementedException();
        }

        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        public bool OpenShop()
        {
            throw new NotImplementedException();
        }

        public void RateProduct(User user, int rate)
        {
            throw new NotImplementedException();
        }

        public void RateStore(User user, int rate)
        {
            throw new NotImplementedException();
        }

        public User Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProduct()
        {
            throw new NotImplementedException();
        }

        public bool RemoveProduct(ShopProduct product)
        {
            throw new NotImplementedException();
        }

        public void Report()
        {
            throw new NotImplementedException();
        }

        public List<Product> Search(string searchString, List<ProductFilter> filters = null)
        {
            throw new NotImplementedException();
        }

        public Product SearchProduct(string searchString)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(User user, string message)
        {
            throw new NotImplementedException();
        }

        public void ShutdownShop(Shop shop)
        {
            throw new NotImplementedException();
        }

        public void ViewHistory(User user)
        {
            throw new NotImplementedException();
        }

        public void ViewHistory(Shop shop)
        {
            throw new NotImplementedException();
        }

        public void WatchHistory()
        {
            throw new NotImplementedException();
        }
    }
}
