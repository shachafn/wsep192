namespace DomainLayer
{
    public class ShopProduct
    {
        public ShopProduct(Product product, double price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Product Product { get; }

        public override string ToString()
        {
            return "Product: " + Product.ToString() + "\nPrice: " + Price.ToString() + "\nQuantity: " + Quantity.ToString();
        }
    }

}