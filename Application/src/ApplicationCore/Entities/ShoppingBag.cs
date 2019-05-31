using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApplicationCore.Entitites
{
    [Table("ShoppingBags")]
    public class ShoppingBag : BaseEntity
    {
        [ForeignKey("Users")]
        public Guid UserGuid { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public ShoppingBag(Guid userGuid)
        {
            UserGuid = userGuid;
            ShoppingCarts = new List<ShoppingCart>();
        }

        public bool IsEmpty()
        {
            return ShoppingCarts.Count==0;
        }
    }
}
