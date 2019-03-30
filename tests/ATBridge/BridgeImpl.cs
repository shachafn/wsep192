﻿using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;
using ServiceLayer.Services;

namespace ATBridge
{
    public class BridgeImpl : IBridge
    {
        private readonly UserService _userService;
        private readonly ShopService _shopService;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly AdminService _adminService;
        private string _errorStr;

        public BridgeImpl()
        {
            _userService = new UserService();
            _shopService = new ShopService();
            _shoppingCartService = new ShoppingCartService();
            _adminService = new AdminService();
            _errorStr = "";
        }

        public Guid AddProductToShop(string name, string category, double price, int quantity, Guid shopGuid)
        {
            return _shopService.AddProductToShop(name, category, price, quantity,shopGuid);
        }

        public bool AddProductToShoppingCart(Guid productGuid, Guid shopOfCartGuid, string username)
        {
            return _shoppingCartService.AddProductToShoppingCart(productGuid, productGuid, username);
        }

        public bool AddShopManager(Guid shopGuid, string ownerUsername, string managerToAddUsername, List<string> priviliges)
        {
            return _shopService.AddShopManager(shopGuid, ownerUsername, managerToAddUsername, priviliges);
        }

        public bool AddShopOwner(Guid shopGuid, string ownerUsername, string managerToAddUsername)
        {
            return _shopService.AddShopOwner(shopGuid, ownerUsername, managerToAddUsername);
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, string ownerUsername)
        {
            return _shopService.CascadeRemoveShopOwner(shopGuid, ownerUsername);
        }

        public bool ChangePurchasedProductAmount(string username, Guid shopOfCartGuid, Guid productGuid, int newAmount)
        {
            return _shoppingCartService.ChangePurchasedProductAmount(username, shopOfCartGuid, productGuid, newAmount);
        }

        public bool EditProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _shopService.EditProduct(shopGuid, productGuid, newPrice, newQuantity);
        }

        public IEnumerable<Guid> GetAllProducts(string username, Guid shopOfCartGuid)
        {
            return _shoppingCartService.GetAllProducts(username, shopOfCartGuid);
        }

        public bool Login(string username, string password)
        {
            return _userService.Login(username, password);
        }

        public bool Logout(string username)
        {
            return _userService.Logout(username);
        }

        public Guid OpenShop(string username)
        {
            return _userService.OpenShop(username, out _errorStr);
        }

        public bool PurchaseBag(string username)
        {
            return _userService.PurchaseBag(username);
        }

        public User Register(string username, string password)
        {
            return _userService.Register(username, password, out _errorStr);
        }

        public bool RemoveProduct(Guid productGuid, Guid shopOfCartGuid, string username)
        {
            return _shoppingCartService.RemoveProduct(productGuid, shopOfCartGuid, username);
        }

        public bool RemoveProductFromShop(Guid productGuid, Guid shopGuid)
        {
            return _shopService.RemoveProductFromShop(productGuid, shopGuid);
        }

        public bool RemoveShopManager(Guid shopGuid, string ownerUsername)
        {
            return _shopService.RemoveShopManager(shopGuid, ownerUsername);
        }

        public IEnumerable<Product> SearchProduct(Guid shopGuid, string productName)
        {
            return _shopService.SearchProduct(shopGuid, productName);
        }
    }
}
