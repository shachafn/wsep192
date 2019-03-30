using System.Collections.Generic;


namespace DomainLayer
{

    public class ShoppingCart
    {

        private Shop _shop;
        private List<ShopProduct> _shopProducts;

        public ShoppingCart(Shop shop)
        {
            _shop = shop;
            _shopProducts = new List<ShopProduct>();
        }
        public Shop Shop { get; }
        public List<ShopProduct> ShopProducts { get; set; }

        public void AddProduct(ShopProduct newShopProduct)
        {
            bool product_exists = false;
            foreach (ShopProduct sp in ShopProducts)
            {
                if (sp.Product.Equals(newShopProduct.Product))
                {
                    product_exists = true;
                    sp.Quantity += newShopProduct.Quantity;
                }
            }
            if (!product_exists)
                _shopProducts.Add(newShopProduct);
        }
        public void RemoveProduct(Product p, int amountToRemove = int.MaxValue)
        {
            foreach (ShopProduct sp in ShopProducts)
            {
                if (sp.Product.Equals(p))
                {
                    if (sp.Quantity <= amountToRemove)
                    {
                        ShopProducts.Remove(sp);
                    }
                    else
                    {
                        sp.Quantity -= amountToRemove;
                    }
                    break;
                }
            }
        }
        public bool HasProduct(Product p)
        {
            foreach (ShopProduct sp in ShopProducts)
            {
                if (sp.Product.Equals(p))
                {
                    return true;
                }
            }
            return false;
        }
        public List<ShopProduct> GetAllProducts()
        {
            return ShopProducts;
        }
        public double CalculateTotalPrice()
        {
            //TODO: next version apply here the discount policy 
            double total = 0;
            foreach (ShopProduct sp in ShopProducts)
            {
                total += (sp.Quantity * sp.Price);
            }
            return total;
        }


        public override string ToString()
        {
            string result = Shop.Owner.ToString();
            foreach (ShopProduct sp in ShopProducts)
            {
                result += "\n" + sp.ToString();
            }
            return result;
        }
    }

}