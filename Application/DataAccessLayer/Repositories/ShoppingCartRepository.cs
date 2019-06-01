using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using DataAccessLayer.IRepositories;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCartDAO>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationContext context) : base(context)
        {
        }

    }
}
