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

        public Guid Guid { get; private set; }
        private string FieldName { get; }
        private object Value { get; }
        private string Description { get; }


        public UserPurchasePolicy(string fieldName,object value,string description)
        {
            Guid = Guid.NewGuid();

            FieldName = fieldName;
            Value = value;
            Description = description;
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
