using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{

    
    [Table("Shops")]
    public class Shop : BaseEntity
    {
        [ForeignKey("ShopOwners")]
        public ShopOwner Creator { get; }
        public ICollection<ShopOwner> Owners { get; set; }
        public ICollection<ShopOwner> Managers { get; set; }
        public ICollection<ShopProduct> ShopProducts { get; set; }
        public enum ShopStateEnum { Active, Closed, PermanentlyClosed };
        [Required]
        public ShopStateEnum ShopState { get; set; }

        public ICollection<Tuple<Guid,Product,int>> UsersPurchaseHistory { get; set; }

        public ICollection<IPurchasePolicy> PurchasePolicies { get; set; }
        
        public ICollection<IDiscountPolicy> DiscountPolicies { get; set; }
        public string ShopName { get; }


        public Shop(Guid ownerGuid)
        {
            Creator = new ShopOwner(ownerGuid, Guid);
            Owners = new List<ShopOwner>();
            Managers = new List<ShopOwner>();
            ShopProducts = new List<ShopProduct>();
            ShopState = ShopStateEnum.Active;
            UsersPurchaseHistory = new List<Tuple<Guid, Product, int>>();
            PurchasePolicies = new List<IPurchasePolicy>();
            DiscountPolicies = new List<IDiscountPolicy>();
            ShopName = ownerGuid.ToString();
        }

        public Shop(Guid ownerGuid, string name) : this (ownerGuid)
        {
            if (name != null && name.Length > 0)
                ShopName = name;
        }
    }
}
