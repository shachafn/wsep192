using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Interfaces.DAL;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Extension_Methods;
using DomainLayer.Users.States;
using System;
using System.Linq;

namespace DomainLaye.Users.States
{
    public class StateBuilder
    {
        private IUnitOfWork _unitOfWork;

        public StateBuilder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IAbstractUserState BuildState(string newState, IUser user, IUnitOfWork unitOfWork)
        {
            switch(newState)
            {
                case AdminUserState.AdminUserStateString:
                    return BuildAdminState(user, unitOfWork);
                case BuyerUserState.BuyerUserStateString:
                    return BuildBuyerState(user, unitOfWork);
                case SellerUserState.SellerUserStateString:
                    return BuildSellerState(user, unitOfWork);
                default:
                    throw new ArgumentException($"newState parameter does not match any state.");
            }
        }

        private IAbstractUserState BuildSellerState(IUser user, IUnitOfWork unitOfWork)
        {
            var res = new SellerUserState(unitOfWork);
            BuildShopsOwned(res, user);
            return res;
        }

        private void BuildShopsOwned(SellerUserState res, IUser user)
        {
            res.ShopsOwned = _unitOfWork.ShopRepository.FindAll().Where(shop => shop.IsOwner(user.Guid)).ToList();
        }

        private IAbstractUserState BuildBuyerState(IUser user, IUnitOfWork unitOfWork)
        {
            var res = new BuyerUserState(unitOfWork);
            //Build purchase history maybe
            return res;
        }

        private IAbstractUserState BuildAdminState(IUser user, IUnitOfWork unitOfWork)
        {
            var res = new AdminUserState(unitOfWork);
            return res;
        }
    }
}
