using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entitites;

namespace DataAccessLayer.DAOs
{

    
    [Table("Shops")]
    public class ShopDAO 
    {
        [Key]
        public Guid Guid { get; set; }
        [ForeignKey("ShopOwners")]
        public ShopOwnerDAO Creator { get; }
        public ICollection<ShopOwnerDAO> Owners { get; set; }
        public ICollection<ShopOwnerDAO> Managers { get; set; }
        public ICollection<ShopProductDAO> ShopProducts { get; set; }
        public enum ShopStateEnum { Active, Closed, PermanentlyClosed };
        [Required]
        public ShopStateEnum ShopState { get; set; }

        public ICollection<RecordsPerCartDAO> UsersPurchaseHistory { get; set; }

       // public ICollection<IPurchasePolicy> PurchasePolicies { get; set; }
        
        //public ICollection<IDiscountPolicy> DiscountPolicies { get; set; }
        public string ShopName { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ShopDAO()
        { }

        public ShopDAO(Shop shop)
        {
            shop.Guid = shop.GetGuid();
            foreach (ShopOwner o in shop.Owners)
                Owners.Add(new ShopOwnerDAO(o));
            foreach (ShopOwner o in shop.Managers)
                Managers.Add(new ShopOwnerDAO(o));
            //foreach (ShopProduct sp in shop.ShopProducts)
              //ShopProducts.Add(new ShopProductDAO(sp.Guid,null,sp.Quantity,sp.Price));
            if(shop.ShopState == Shop.ShopStateEnum.Active)
            {
                ShopState = ShopStateEnum.Active;
            }
            if (shop.ShopState == Shop.ShopStateEnum.Closed)
            {
                ShopState = ShopStateEnum.Closed;
            }
            if (shop.ShopState == Shop.ShopStateEnum.PermanentlyClosed)
            {
                ShopState = ShopStateEnum.PermanentlyClosed;
            }
            ShopName = shop.ShopName;
        }
    }
}
