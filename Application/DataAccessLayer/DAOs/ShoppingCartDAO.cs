using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.DAOs
{
    [Table("Carts")]
    public class ShoppingCartDAO
    {
        [Key]
        public Guid cartGuid { get; set; }
        [ForeignKey("Users")]
        public Guid UserGuid { get; set; }
        //public BaseUserDAO BaseUser { get; set; }

        [ForeignKey("Shops")]
        public Guid ShopGuid { get; set; }
        //public ShopDAO Shop { get; set; }

        [ForeignKey("CartRecords")]
        public ICollection<RecordsPerCartDAO> RecordsGuids { get; set; } // Shop product and quantity that was purchased.
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ShoppingCartDAO()
        {

        }

        public ShoppingCartDAO(ShoppingCart cart)
        {
            //TODO: Find user in USERS
            //TODO: Find Shops in shop
            foreach (Tuple<Guid, int> tuple in cart.PurchasedProducts)
            {
                //Find tuple.guid in products
                //NEW cartRecord
            }
        }

       
    }
}
