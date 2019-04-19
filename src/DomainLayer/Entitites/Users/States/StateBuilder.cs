using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class StateBuilder
    {
        public AbstractUserState BuildState(string newState, User user)
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

        private AbstractUserState BuildSellerState(User user)
        {
            var res = new SellerUserState();
            BuildShopsOwned(res, user);
            return res;
        }

        private void BuildShopsOwned(SellerUserState res, User user)
        {
            res.ShopsOwned = DomainData.ShopsCollection.Where(shop => shop.IsOwner(user.Guid)).ToList();
        }

        private AbstractUserState BuildBuyerState(User user)
        {
            var res = new BuyerUserState();
            //Build purchase history maybe
            return res;
        }

        private AbstractUserState BuildAdminState(User user)
        {
            var res = new AdminUserState();
            return res;
        }
    }
}
