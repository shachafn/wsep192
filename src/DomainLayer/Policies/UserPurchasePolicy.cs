using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using DomainLayer.Data.Entitites;
using DomainLayer.Operators.ArithmeticOperators;

namespace DomainLayer.Policies
{
    class UserPurchasePolicy : IPurchasePolicy
    {
        private IUser IdealUser { get; }
        private Func<Tuple<IUser, IUser>, bool> Predicate { get; }

        public UserPurchasePolicy(IUser idealUser, Func<Tuple<IUser, IUser>, bool> predicate)
        {
            IdealUser = idealUser;
            Predicate = predicate;
        }
        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, IUser inputUser)
        {
            Tuple<IUser, IUser> inputForPredicate = new Tuple<IUser, IUser>(IdealUser, inputUser);
            return Predicate(inputForPredicate);
        }
    }
}
