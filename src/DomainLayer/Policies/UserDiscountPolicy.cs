using System;
using System.Reflection;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;


namespace DomainLayer.Policies
{
    public class UserDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; private set; }

        private string FieldName;
        private object ExpectedValue;
        private string Description { get; }


        public UserDiscountPolicy(string fieldName, object expectedValue,string description)
        {
            Guid = Guid.NewGuid();

            FieldName = fieldName;
            ExpectedValue = expectedValue;
            Description = description;
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