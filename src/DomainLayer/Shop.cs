using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer
{

    public class Shop
    {
        public static Dictionary<Guid, Shop> _shops = new Dictionary<Guid, Shop>();
        public static Shop GetShopByGuid(Guid guid) => _shops[guid];

        private Guid _guid;
        private ShopOwner _owner;
        private List<Tuple<User, string>> _messages;
        private List<ShopProduct> _shopProducts;
        private List<Tuple<User, ShoppingCart>> _purchaseHistory;
        private double _rate;
        private int _sumOfRates;
        private int _numberOfRates;


        public Shop(ShopOwner shopOwner)
        {
            _guid = Guid.NewGuid();

            _owner = shopOwner;
            _messages = new List<Tuple<User, string>>();
            _shopProducts = new List<ShopProduct>();
            _purchaseHistory = new List<Tuple<User, ShoppingCart>>();
            _rate = 0;
            _sumOfRates = 0;
            _numberOfRates = 0;
            _shops.Add(_guid, this);
        }

        public ShopOwner Owner { get; }
        public List<ShopProduct> ShopProducts { get; }
        public double Rate { get; }

        public void RateShop(User user, int rate)
        {
            if (CanRateShop(user) && IsValidRate(rate))
            {
                _sumOfRates += rate;
                _numberOfRates++;
                rate = _sumOfRates / _numberOfRates;
            }

        }
        private bool CanRateShop(User user)
        {
            return user.IsLogged() && user.HasPurchasedInShop(this);
        }
        private bool IsValidRate(int rate)
        {
            return rate >= 1 && rate <= 5;
        }
        public void AddProduct(Product product, double price, int quantity)
        {
            ShopProduct newShopProduct = new ShopProduct(product, price, quantity);
            _shopProducts.Add(newShopProduct);
        }
        public void RemoveProduct(Product product)
        {
            ShopProduct toRemove = SearchProduct(product);
            if (toRemove != null)
                _shopProducts.Remove(toRemove);
        }
        public void RemoveProduct(Guid productGuid)
        {
            ShopProduct toRemove = _shopProducts.FirstOrDefault(prod => prod.Product.Guid.Equals(productGuid));
            if (toRemove != null)
                _shopProducts.Remove(toRemove);
        }
        private ShopProduct SearchProduct(Product product)
        {
            foreach (ShopProduct sp in _shopProducts)
            {
                if (sp.Product.Equals(product))
                    return sp;
            }
            return null;
        }
        public void EditProduct(Product product, double price, int quantity)
        {
            ShopProduct toEdit = SearchProduct(product);
            if (toEdit == null) return;
            toEdit.Price = price;
            toEdit.Quantity = quantity;
        }

        public void EditProduct(Guid productGuid, double price, int quantity)
        {
            var toEdit = _shopProducts.FirstOrDefault(prod => prod.Product.Guid.Equals(productGuid));
            if (toEdit == null) return;
            toEdit.Price = price;
            toEdit.Quantity = quantity;
        }

        public void SendMessage(User user, string message)
        {
            if (!user.IsLogged()) return;
            _messages.Add(Tuple.Create(user, message));
        }
        public List<ShoppingCart> GetPurchaseHistory(User user)
        {
            List<ShoppingCart> toReturn = new List<ShoppingCart>();
            foreach (Tuple<User, ShoppingCart> purchase in _purchaseHistory)
            {
                if ((user.Username).Equals(purchase.Item1.Username))
                    toReturn.Add(purchase.Item2);
            }
            return toReturn;

        }
        public IEnumerable<Product> SearchProducts(string searchString)
        {
            List<Product> toReturn = new List<Product>();
            foreach (ShopProduct sp in _shopProducts)
            {
                if (sp.Product.Name.Contains(searchString) || sp.Product.Category.Contains(searchString))
                    toReturn.Add(sp.Product);
            }
            return toReturn;
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "Owner: " + _owner + "\nRate: " + _rate + "\nProducts: " + _shopProducts.ToString();
        }
    }
}
