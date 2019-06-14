using System;
using System.Reflection;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace DomainLayer.Policies
{
    public class UserDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; private set; }

        private string FieldName;
        private object ExpectedValue;
        public int DiscountPercentage { get; set; }
        public string Description { get; }

       
        public UserDiscountPolicy() { }// Empty constructor for ref
        public UserDiscountPolicy(string fieldName, object expectedValue,int discountPercentage,string description)
        {
            //field1 = field name
            //field2 = expected value
            //field3 = discount percentage
            //field4 = description
            Guid = Guid.NewGuid();

            FieldName = fieldName;
            ExpectedValue = expectedValue;
            DiscountPercentage = discountPercentage;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            foreach (PropertyInfo property in user.GetType().GetProperties())
            {
                if (property.Name == FieldName)
                {
                    return ExpectedValue.Equals(property.GetValue(user)) ? true : false;
                }
            }
            return false;
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            if (CheckPolicy(cart, productGuid, quantity, user, unitOfWork))
            {
                double totalSum = CalculateSumBeforeDiscount(cart);
                double discountValue = -totalSum * (DiscountPercentage / 100);
                if (discountValue == 0) return null;
                Product discountProduct = new Product("Discount - user", "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProduct, discountValue, 1);
                return new Tuple<ShopProduct, int>(discountRecord, 1);
            }
            return null;
        }

        private double CalculateSumBeforeDiscount(ShoppingCart cart)
        {
            double totalSum = 0;
            foreach (Tuple<ShopProduct, int> record in cart.PurchasedProducts)
            {
                Shop shop = null; //TODO-FIX DomainData.ShopsCollection[cart.ShopGuid];
                foreach (ShopProduct productInShop in shop.ShopProducts)
                {
                    if (productInShop.Guid.Equals(record.Item1.Guid))
                    {
                        totalSum += (productInShop.Price * record.Item2);
                        break;
                    }
                }
            }
            return totalSum;
        }
    }

}