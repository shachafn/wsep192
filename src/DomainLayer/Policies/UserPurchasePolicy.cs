using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.ArithmeticOperators;

namespace DomainLayer.Policies
{
    class UserPurchasePolicy : IPurchasePolicy
    {
        private BaseUser IdealUser { get; }
        //private Func<Tuple<BaseUser, BaseUser>, bool> Predicate { get; }

        public UserPurchasePolicy(BaseUser idealUser, Func<Tuple<BaseUser, BaseUser>, bool> predicate)
        {
            IdealUser = idealUser;
            //Predicate = predicate;
        }
        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser inputUser)
        {
            throw new NotImplementedException();
            //Tuple<BaseUser, BaseUser> inputForPredicate = new Tuple<BaseUser, BaseUser>(IdealUser, inputUser);
            //return Predicate(inputForPredicate);
        }
    }
}
