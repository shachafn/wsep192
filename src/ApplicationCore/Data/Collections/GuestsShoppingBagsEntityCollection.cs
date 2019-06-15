using ApplicationCore.Entitites;
using System;
using System.Linq;

namespace ApplicationCore.Data.Collections
{
    public class GuestsShoppingBagsEntityCollection : EntityCollection<ShoppingBag>
    {
        public ShoppingBag GetShoppingBagAndCreateIfNeeded(Guid userGuid)
        {
            if (!_entitiesCollection.ContainsKey(userGuid))
                _entitiesCollection.Add(userGuid, new ShoppingBag(userGuid));
            return _entitiesCollection[userGuid];
        }
    }
}
