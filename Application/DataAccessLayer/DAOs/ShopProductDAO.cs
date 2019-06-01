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
        public Guid Id { get; set; }

        //[ForeignKey("Products")]
        public ProductDAO Product { get; set; }

        [Required(ErrorMessage = "Quantity required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price required")]
        public double Price { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ShopProductDAO() { }

        public ShopProductDAO(Guid thisGuid, ProductDAO product, int quantity, double price)
        {
            this.Id = thisGuid;
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        public override string ToString()
        {
            return $"Guid - {Id}, Product - {Product}, Price - {Price}, Quantity - {Quantity}";
        }
    }
}
