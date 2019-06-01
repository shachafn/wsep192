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
       // [ForeignKey("Users")]
        public BaseUserDAO UserGuid { get; set; }

        //[ForeignKey("Shops")]
        public ShopDAO ShopGuid { get; set; }

        //[ForeignKey("CartRecords")]
        public ICollection<RecordsPerCartDAO> PurchasedProducts { get; set; } // Shop product and quantity that was purchased.
        [Timestamp]
        public byte[] RowVersion { get; set; }

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
