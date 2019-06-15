using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace DomainLayer.Policies
{
    public interface IPurchasePolicy
    {

        bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork);
        Guid Guid { get; }
        string Description { get; }
    }
}
