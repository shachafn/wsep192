namespace ApplicationCore.Entitites
{
    public class ShopProduct : BaseEntity
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

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
