using System;
using System.Reflection;
//using System.Reflection;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace DomainLayer.Policies
{
    public class UserPurchasePolicy : IPurchasePolicy
    {

        public Guid Guid { get; set; }
        public string FieldName { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }

        public UserPurchasePolicy() { } //Empty constructor for ref
        public UserPurchasePolicy(string fieldName,object value,string description)
        {
            Guid = Guid.NewGuid();

            FieldName = fieldName;
            Value = value;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser inputUser, IUnitOfWork unitOfWork)
        {
            if (inputUser==null) return false;
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
