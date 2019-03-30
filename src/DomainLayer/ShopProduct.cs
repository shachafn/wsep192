﻿namespace DomainLayer
{

    public class ShopProduct
    {
        private Product _product;
        private double _price;
        private int _quantity;
        public ShopProduct(Product product, double price, int quantity)
        {
            this.Product = product;
            this.Price = price;
            this.Quantity = quantity;
        }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Product Product { get; }

        public override string ToString()
        {
            return "Product: " + _product.ToString() + "\nPrice: " + _price.ToString() + "\nQuantity: " + _quantity.ToString();
        }
    }

}