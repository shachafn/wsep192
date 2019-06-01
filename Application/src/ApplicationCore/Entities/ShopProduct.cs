using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
   // [Table("ShopProducts")]
    public class ShopProduct : BaseEntity
    {
        //[Key, Column(Order = 0)]
        public Guid thisGuid;
       // [ForeignKey("Products")]
        public Product Product { get; set; }

        //[Required(ErrorMessage = "Quantity required")]
        public int Quantity { get; set; }
        //[Required(ErrorMessage = "Price required")]
        public double Price { get; set; }
      //  [Timestamp]
        public byte[] RowVersion { get; set; }
        public ShopProduct()
        {
            thisGuid = base.GetGuid();
            
        }
        public ShopProduct(Product product, double price, int quantity)
        {
            thisGuid = base.GetGuid();
            Product = product;
            Price = price;
            Quantity = quantity;
        }
        public ShopProduct Clone()
        {
            return new ShopProduct(Product, Price, Quantity);
        }
        public override string ToString()
        {
            return $"Guid - {Guid}, Product - {Product}, Price - {Price}, Quantity - {Quantity}";
        }
    }
}
