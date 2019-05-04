using System;
using System.Reflection;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;


namespace DomainLayer.Policies
{
    public class UserDiscountPolicy : IDiscountPolicy
    {
        private string FieldName;
        private object ExpectedValue;

        public UserDiscountPolicy(string fieldName, object expectedValue)
        {
            FieldName = fieldName;
            ExpectedValue = expectedValue;
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
            return true;
        }
    }

}