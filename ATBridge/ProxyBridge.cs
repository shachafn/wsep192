using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;

namespace ATBridge
{
    public class ProxyBridge : IBridge
    {
        private IBridge _real;

        public ProxyBridge()
        {
            _real = null;
        }

        public void SetRealBridge(IBridge impl)
        {
            if (_real == null)
                _real = impl;
        }

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
        }

        public void GetAllProducts()
        {
        }

        public void GetPurchaseHistory()
        {
        }

        public bool Login(string username, string password)
        {
            return false;
        }

        public bool Logout()
        {
            return false;
        }

        public bool OpenShop()
        {
            return false;
        }

        public void RateProduct(User user, int rate)
        {
        }

        public void RateStore(User user, int rate)
        {

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
        }

        public void ShutdownShop(Shop shop)
        {
        }

        public void ViewHistory(User user)
        {
        }

        public void ViewHistory(Shop shop)
        {
        }

        public void WatchHistory()
        {
        }
    }
}
