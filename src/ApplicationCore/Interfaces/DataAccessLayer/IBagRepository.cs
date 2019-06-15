using ApplicationCore.Entitites;
using System;

namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface IBagRepository : IRepository<ShoppingBag>
    {
        ShoppingBag GetShoppingBagAndCreateIfNeeded(Guid userGuid);
        ShoppingCart GetShoppingCartAndCreateIfNeeded(Guid userGuid, Guid shopGuid);
    }
}