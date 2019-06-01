using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataAccessLayer.DAOs
{
    [Table("ShoppingBags")]
    public class ShoppingBagDAO
    {
        public Guid UserGuid { get; set; }
        public BaseUserDAO User { get; set; }
        public ICollection<ShoppingCartDAO> ShoppingCarts { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }


        public ShoppingBagDAO()  { }

        public ShoppingBagDAO(ShoppingBag shoppingBag)
        {
            //find user with db
            foreach (ShoppingCart cart in shoppingBag.ShoppingCarts)
                ShoppingCarts.Add(new ShoppingCartDAO(cart));
        }
    }
}
