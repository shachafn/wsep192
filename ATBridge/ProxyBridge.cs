using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;

namespace ATBridge
{
    class ProxyBridge : IBridge
    {
        public bool AddProduct()
        {
            return false;
        }

        public bool AddProduct(ShopProduct product)
        {
            return false;
        }

        public bool AddReview(User user, string text)
        {
            return false;
        }

        public bool EditProduct(Product product, double price, int quantity)
        {
            return false;
        }

        public void EditProfile()
        {
            throw new NotImplementedException();
        }

        public void GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public void GetPurchaseHistory()
        {
            throw new NotImplementedException();
        }

        public bool Login(string username, string password)
        {
            return false;
        }

        public bool Logout()
        {
            return false;
        }

        public void OpenShop()
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
            return null;
        }

        public bool RemoveProduct()
        {
            return false;
        }

        public bool RemoveProduct(ShopProduct product)
        {
            return false;
        }

        public void Report()
        {
            throw new NotImplementedException();
        }

        public List<Product> Search(string searchString, List<ProductFilter> filters = null)
        {
            return null;
        }

        public Product SearchProduct(string searchString)
        {
            return null;
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
