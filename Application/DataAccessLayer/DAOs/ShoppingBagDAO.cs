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
        //[ForeignKey("Users")]
        public BaseUserDAO User { get; set; }

        //[ForeignKey("Carts")]
        public ICollection<ShoppingCartDAO> ShoppingCarts { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ShoppingBagDAO(ShoppingBag shoppingBag)
        {
            //find user with db
            foreach (ShoppingCart cart in shoppingBag.ShoppingCarts)
                ShoppingCarts.Add(new ShoppingCartDAO(cart));
        }
    }
}
