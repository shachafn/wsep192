using System;
using System.Collections.Generic;
using System.Linq;

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
        public void RemoveProduct(Guid productGuid)
        {
            var product = _shopProducts.FirstOrDefault(prod => prod.Product.ProductGuid.Equals(productGuid));
            if (product != null)
                _shopProducts.Remove(product);
        }
        public void EditProduct(Guid productGuid, int newQuantity)
        {
            var product = _shopProducts.FirstOrDefault(prod => prod.Product.ProductGuid.Equals(productGuid));
            product.Quantity = newQuantity;
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
        public bool PurchaseCart(User user)
        {
            //TODO: Send to external payment method
            for (int i = 0; i < ShopProducts.Count; i++)
            {
                ShopProduct sp = ShopProducts[i];
                if (Shop.SearchProduct(sp.Product).Quantity < sp.Quantity)
                {
                    //Cant purchase,the selected amount is unavailable
                    //return the items to the shop
                    for (int j = 0; j < i; j++)
                    {
                        ShopProduct sp1 = ShopProducts[j];
                        Shop.EditProduct(sp1.Product, sp1.Price, Shop.SearchProduct(sp1.Product).Quantity + sp1.Quantity);
                    }
                    return false;
                }
                Shop.EditProduct(sp.Product, sp.Price, Shop.SearchProduct(sp.Product).Quantity - sp.Quantity);

            }
            _shop.AddToPurchaseHistory(user, this);
            ShoppingBag addToUserHistory = new ShoppingBag();
            addToUserHistory.AddCart(this);
            user.AddToPurchaseHistory(addToUserHistory);
            return true;
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