using DataAccessLayer.DAOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    [Table("ShopProducts")]
    public class ShopProductDAO
    {
        [Key, Column(Order = 0)]
        public Guid thisGuid;
        //[ForeignKey("Products")]
        public ProductDAO Product { get; set; }

        [Required(ErrorMessage = "Quantity required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price required")]
        public double Price { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ShopProductDAO(Guid thisGuid, ProductDAO product, int quantity, double price, byte[] rowVersion)
        {
            this.thisGuid = thisGuid;
            Product = product;
            Quantity = quantity;
            Price = price;
            RowVersion = rowVersion;
        }

        public ShopProductDAO(ShopProduct shopProduct)
        {
            thisGuid = shopProduct.GetGuid();
            Product = new ProductDAO(shopProduct.Product);
            Quantity = shopProduct.Quantity;
            Price = shopProduct.Price;
        }

        public override string ToString()
        {
            return $"Guid - {thisGuid}, Product - {Product}, Price - {Price}, Quantity - {Quantity}";
        }
    }
}
