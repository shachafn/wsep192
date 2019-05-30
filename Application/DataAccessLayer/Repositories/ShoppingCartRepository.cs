using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entitites;

namespace DataAccessLayer.Repositories
{
    class ShoppingCartRepository : RepositoryBase<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
