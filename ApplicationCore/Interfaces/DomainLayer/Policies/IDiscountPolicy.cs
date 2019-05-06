using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System;


namespace DomainLayer.Policies
{
    public interface IDiscountPolicy
    {
        Guid Guid { get; }
        int DiscountPercentage { get; set; }
        bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user);
        void ApplyPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user);
    }
}
