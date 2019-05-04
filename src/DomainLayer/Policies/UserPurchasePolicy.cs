using System;
using System.Collections.Generic;
using System.Reflection;
//using System.Reflection;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace DomainLayer.Policies
{
    class UserPurchasePolicy : IPurchasePolicy
    {

        private string FieldName { get; }
        private object Value { get; }
        

        public UserPurchasePolicy(string fieldName,object value)
        {
            FieldName = fieldName;
            Value = value;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser inputUser)
        {
            foreach(PropertyInfo property in inputUser.GetType().GetProperties())
            {
                if(property.Name == FieldName)
                {
                    return Value.Equals(property.GetValue(inputUser)) ? true : false;
                }
            }
            return true;
        }
    }
}
