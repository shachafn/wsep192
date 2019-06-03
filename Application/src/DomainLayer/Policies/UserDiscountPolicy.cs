using System;
using System.Reflection;
using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DAL;
using DomainLayer.Extension_Methods;

namespace DomainLayer.Policies
{
    public class UserDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; private set; }

        private string FieldName;
        private object ExpectedValue;
        public int DiscountPercentage { get; set; }
        private string Description { get; }


        private IUnitOfWork _unitOfWork;
       
        public UserDiscountPolicy(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }// Empty constructor for ref
        public UserDiscountPolicy(string fieldName, object expectedValue,int discountPercentage,string description, IUnitOfWork unitOfWork)
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

            _unitOfWork = unitOfWork;
        }

        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
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

        public void ApplyPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            if (CheckPolicy(ref cart, productGuid, quantity, user))
            {
                double totalSum = CalculateSumBeforeDiscount(cart);
                double discountValue = -totalSum * (DiscountPercentage / 100);
                if (discountValue == 0) return;
                Product discountProduct = new Product("Discount - user", "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProduct, discountValue, 1);
                cart.AddProductToCart(discountRecord.Guid, 1);
            }
        }

        private double CalculateSumBeforeDiscount(ShoppingCart cart)
        {
            double totalSum = 0;
            foreach (Tuple<Guid, int> record in cart.PurchasedProducts)
            {
                Shop shop = _unitOfWork.ShopRepository.FindById(cart.ShopGuid);
                foreach (ShopProduct productInShop in shop.ShopProducts)
                {
                    if (productInShop.Guid.Equals(record.Item1))
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