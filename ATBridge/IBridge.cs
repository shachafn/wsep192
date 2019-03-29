using DomainLayer;
using System;
using System.Collections.Generic;

namespace ATBridge
{
    public interface IBridge
    {
        // User
        bool Login(string username, string password);
        bool Logout();
        User Register(string username, string password);
        List<Product> Search(string searchString, List<ProductFilter> filters = null);
        bool OpenShop();
        void WatchHistory();
        void EditProfile();

        // Shopping Cart
        bool AddProduct();
        bool RemoveProduct();
        void GetAllProducts();

        //shop
        void RateStore(User user, int rate);
        bool AddProduct(ShopProduct product);
        bool RemoveProduct(ShopProduct product);
        void SendMessage(User user, string message);
        bool EditProduct(Product product, double price, int quantity);
        void GetPurchaseHistory();
        Product SearchProduct(string searchString);
        bool AddReview(User user, string text);
        void RateProduct(User user, int rate);

        // Admin
        void Report();
        void ViewHistory(User user);
        void ViewHistory(Shop shop);
        void ShutdownShop(Shop shop);

    }
}
