using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccessLayer.DAOs
{
    [Table("CartRecords")]
    public class RecordsPerCartDAO
    {
        //[ForeignKey("Carts")]
        public ShoppingCartDAO Cart { get; set; }
        //[ForeignKey("Products")]
        public ProductDAO Product { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int PurchasedQuantity { get; set; }

        public RecordsPerCartDAO(ShoppingCartDAO cart, ProductDAO product, int purchasedQuantity)
        {
            Cart = cart;
            Product = product;
            PurchasedQuantity = purchasedQuantity;
        }
    }
}
