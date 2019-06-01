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
        public Guid CartGuid { get; set; }
        public ShoppingCartDAO Cart { get; set; }

        public Guid ProductGuid { get; set; }
        public ProductDAO Product { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int PurchasedQuantity { get; set; }
        public RecordsPerCartDAO()
        { }
        public RecordsPerCartDAO(ShoppingCartDAO cart, ProductDAO product, int purchasedQuantity)
        {
            Cart = cart;
            Product = product;
            PurchasedQuantity = purchasedQuantity;
        }
    }
}
