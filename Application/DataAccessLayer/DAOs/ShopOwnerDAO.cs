using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.DAOs
{
    [Table("ShopOwners")]
    public class ShopOwnerDAO
    {
        //[ForeignKey("Users")]
        [Key]
        public Guid OwnerGuid { get; set; }
        public BaseUserDAO OwnerBaseUser { get; set; }

        [ForeignKey("ShpOwners")]
        public ShopOwnerDAO Appointer { get; set; } // Guid.Empty for the creator of the shop.

        [ForeignKey("Shops")]
        public ShopDAO Shop { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        //public ICollection<string> Priviliges { get; set; }

        public ShopOwnerDAO()
        { }

        public ShopOwnerDAO(ShopOwner owner)
        {
            //Call db to find object of owner
            //Call db to find object of appointer
            //Call db to find object of shop
             //Priviliges = owner.Priviliges;
        }
    }
}
