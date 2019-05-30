using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entitites;

namespace DataAccessLayer.Repositories
{
    class ShoppingBagRepository : RepositoryBase<ShoppingBag>, IShoppingBagRepository
    {
        public ShoppingBagRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
