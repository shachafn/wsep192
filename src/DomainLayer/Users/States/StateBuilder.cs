using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Extension_Methods;
using DomainLayer.Users.States;
using System;
using System.Linq;

namespace DomainLaye.Users.States
{
    public class StateBuilder
    {
        public IAbstractUserState BuildState(string newState, IUser user)
        {
            switch(newState)
            {
                case AdminUserState.AdminUserStateString:
                    return BuildAdminState(user);
                case BuyerUserState.BuyerUserStateString:
                    return BuildBuyerState(user);
                case SellerUserState.SellerUserStateString:
                    return BuildSellerState(user);
                default:
                    throw new ArgumentException($"newState parameter does not match any state.");
            }
        }

        private IAbstractUserState BuildSellerState(IUser user)
        {
            var res = new SellerUserState();
            BuildShopsOwned(res, user);
            return res;
        }

        private void BuildShopsOwned(SellerUserState res, IUser user)
        {
            res.ShopsOwned = DomainData.ShopsCollection.Where(shop => shop.IsOwner(user.Guid)).ToList();
        }

        private IAbstractUserState BuildBuyerState(IUser user)
        {
            var res = new BuyerUserState();
            //Build purchase history maybe
            return res;
        }

        private IAbstractUserState BuildAdminState(IUser user)
        {
            var res = new AdminUserState();
            return res;
        }
    }
}
