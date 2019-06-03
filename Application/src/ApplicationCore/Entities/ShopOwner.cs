using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    //[Table("ShopOwners")]
    public class ShopOwner
    {
        //[Key]
       // [Column(Order=2)]
        public Guid OwnerGuid { get; set; }

        //[Key]
       // [Column(Order = 2)]
        public Guid AppointerGuid { get; set; } // Guid.Empty for the creator of the shop.

        //[Key]
        //[Column(Order = 3)]
        public Guid ShopGuid { get; set; }
        //[Timestamp]
        public byte[] RowVersion { get; set; }
        public ICollection<string> Priviliges { get; set; }

        public ShopOwner(Guid ownerGuid, Guid appointerGuid, Guid shopGuid, ICollection<string> priviliges = null)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = appointerGuid;
            ShopGuid = shopGuid;
            Priviliges = priviliges ?? new List<string>();
        }

        public ShopOwner(Guid ownerGuid, Guid shopGuid, ICollection<string> priviliges = null)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = Guid.Empty;
            ShopGuid = shopGuid;
            Priviliges = priviliges ?? new List<string>();
        }

        public ShopOwner()
        {
        }
    }
}
