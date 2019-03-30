using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;

namespace ATBridge
{
    public class BridgeImpl : IBridge
    {
        public bool AddProduct()
        {
            return false;
        }

        /*public bool AddProduct(ShopProduct product)
        {
             return false;
        }*/

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
            return;
        }

        public void GetAllProducts()
        {
            return;
        }

        public void GetPurchaseHistory()
        {
            return;
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
            return;
        }

        public void RateStore(User user, int rate)
        {
            return;
        }

        public User Register(string username, string password)
        {
            return null;
        }

        public bool RemoveProduct()
        {
            return false;
        }

        /*public bool RemoveProduct(ShopProduct product)
        {
            return false;
        }*/

        public void Report()
        {
            return;
        }

        /*public List<Product> Search(string searchString, List<ProductFilter> filters = null)
        {
            return null;
        }*/

        public Product SearchProduct(string searchString)
        {
            return null;
        }

        public void SendMessage(User user, string message)
        {
            return;
        }

        public void ShutdownShop(Shop shop)
        {
            return;
        }

        public void ViewHistory(User user)
        {
            return;
        }

        public void ViewHistory(Shop shop)
        {
            return;
        }

        public void WatchHistory()
        {
            return;
        }
    }
}
