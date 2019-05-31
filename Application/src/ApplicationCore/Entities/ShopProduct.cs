using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    [Table("ShopProducts")]
    public class ShopProduct : BaseEntity
    {
        [ForeignKey("Products")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Quantity required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price required")]
        public double Price { get; set; }

        public ShopProduct()
        {

        }
        public ShopProduct(Product product, double price, int quantity)
        {
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
