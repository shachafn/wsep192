using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using DataAccessLayer.IRepositories;

namespace DataAccessLayer.Repositories
{
    class ShoppingBagRepository : RepositoryBase<ShoppingBagDAO>, IShoppingBagRepository
    {
        public ShoppingBagRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
