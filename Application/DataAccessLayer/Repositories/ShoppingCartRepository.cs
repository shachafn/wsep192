using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using ApplicationCore.IRepositories;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCartDAO>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ShoppingCart> FindByCondition(Expression<Func<ShoppingCart, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<ShoppingCart> IRepositoryBase<ShoppingCart>.FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
