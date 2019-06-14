using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using System;


namespace DomainLayer.Policies
{
    public interface IDiscountPolicy
    {
        Guid Guid { get; }
        int DiscountPercentage { get; set; }
        bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork);
        Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork);
        string Description { get; }
    }
}
