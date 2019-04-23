using System;

namespace DomainLayer.Data.Entitites
{
    public class ShopProduct : IEquatable<ShopProduct>
    {
        public Product Product { get; set; }
        public Guid Guid { get => Product.Guid; }
        public int Quantity { get; set; }
        public double Price { get; set; }

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

        //For testing
        public bool Equals(ShopProduct other)
        {
            if (!Product.Equals(other.Product))
                return false;
            if (!Quantity.Equals(other.Quantity))
                return false;
            if (!Price.Equals(other.Price))
                return false;

            return true;
        }
    }
}
