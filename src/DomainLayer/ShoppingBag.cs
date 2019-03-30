using System;
using System.Collections.Generic;

namespace DomainLayer
{

    public class ShoppingBag
    {
        private List<ShoppingCart> _carts;
        public ShoppingBag()
        {
            _carts = new List<ShoppingCart>();
        }
        public List<ShoppingCart> Carts { get; set; }
        public void AddCart(ShoppingCart newCart)
        {
            _carts.Add(newCart);
        }
        public bool HasProduct(Product p)
        {
            foreach (ShoppingCart cart in Carts)
            {
                if (cart.HasProduct(p))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            string result = "";
            foreach (ShoppingCart cart in Carts)
            {
                result += "\n" + cart.ToString();
            }
            return result;
        }

        internal bool HasShop(Shop shop)
        {
            foreach (ShoppingCart cart in Carts)
            {
                if (cart.Shop.Equals(shop))
                    return true;
            }
            return false;
        }
    }

}