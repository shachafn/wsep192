﻿using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;
using ServiceLayer.Services;

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

        public bool Register(string username, string password)
        {
            return _real == null ? false : _real.Register(username, password);
        }

        public bool Login(string username, string password)
        {
            return _real == null ? false : _real.Login(username, password);
        }

        public bool Logout(string username)
        {
            return _real == null ? false : _real.Logout(username);
        }

        public bool OpenShop(string username)
        {
            return _real == null ? false : _real.OpenShop(username);
        }

        public bool PurchaseBag(string username)
        {
            return _real == null ? false : _real.PurchaseBag(username);
        }

        public Guid AddProductToShop(string name, string category, double price, int quantity, Guid shopGuid)
        {
            return _real == null ? Guid.Empty : _real.AddProductToShop(name, category, price, quantity, shopGuid);
        }

        public bool RemoveProductFromShop(Guid productGuid, Guid shopGuid)
        {
            return _real == null ? false : _real.RemoveProductFromShop(productGuid, shopGuid);
        }

        public bool EditProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            return _real == null ? false : _real.EditProduct(shopGuid, productGuid, newPrice, newQuantity);
        }

        public IEnumerable<Product> SearchProduct(Guid shopGuid, string productName)
        {
            return _real?.SearchProduct(shopGuid, productName);
        }

        public bool AddShopOwner(Guid shopGuid, string ownerUsername, string managerToAddUsername)
        {
            return _real == null ? false : _real.AddShopOwner(shopGuid, ownerUsername, managerToAddUsername);
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, string ownerUsername)
        {
            return _real == null ? false : _real.CascadeRemoveShopOwner(shopGuid, ownerUsername);
        }

        public bool AddShopManager(Guid shopGuid, string ownerUsername, string managerToAddUsername, List<string> priviliges)
        {
            return _real == null ? false : _real.AddShopManager(shopGuid, ownerUsername, managerToAddUsername, priviliges);
        }

        public bool RemoveShopManager(Guid shopGuid, string ownerUsername)
        {
            return _real == null ? false : _real.RemoveShopManager(shopGuid, ownerUsername);
        }
    }
}
