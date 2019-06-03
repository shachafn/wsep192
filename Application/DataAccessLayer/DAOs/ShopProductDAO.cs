using DataAccessLayer.DAOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    [Table("ShopProducts")]
    public class ShopProductDAO
    {
        [Key]
        public Guid Id { get; set; }

        public ProductDAO Product { get; set; }

        [ForeignKey("Products")]
        public Guid ProductGuid { get; set; }

        [Required(ErrorMessage = "Quantity required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price required")]
        public double Price { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ShopProductDAO() { }

        public ShopProductDAO(Guid thisGuid, Guid productGuid, int quantity, double price)
        {
            Id = thisGuid;
            ProductGuid = productGuid;
            Quantity = quantity;
            Price = price;
        }

       /* public override string ToString()
        {
            return $"Guid - {Id}, Product - {Product}, Price - {Price}, Quantity - {Quantity}";
        }*/
    }
}
