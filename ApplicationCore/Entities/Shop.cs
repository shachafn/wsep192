﻿using DomainLayer.Policies;
using System;
using System.Collections.Generic;
namespace ApplicationCore.Entitites
{
    public class Shop : BaseEntity
    {
        public ShopOwner Creator { get; }
        public ICollection<ShopOwner> Owners { get; set; }
        public ICollection<ShopOwner> Managers { get; set; }
        public ICollection<ShopProduct> ShopProducts { get; set; }
        public enum ShopStateEnum { Active, Closed, PermanentlyClosed };
        public ShopStateEnum ShopState { get; set; }
        public ICollection<Tuple<Guid,ShopProduct>> UsersPurchaseHistory { get; set; }
        public ICollection<IPurchasePolicy> PurchasePolicies { get; set; }
        public ICollection<IDiscountPolicy> DiscountPolicies { get; set; }
        public Shop(Guid ownerGuid)
        {
            Creator = new ShopOwner(ownerGuid, Guid);
            Owners = new List<ShopOwner>();
            Managers = new List<ShopOwner>();
            ShopProducts = new List<ShopProduct>();
            ShopState = ShopStateEnum.Active;
            UsersPurchaseHistory = new List<Tuple<Guid, ShopProduct>>();
        }
    }
}
