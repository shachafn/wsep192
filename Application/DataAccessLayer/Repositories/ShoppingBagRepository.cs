using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using ApplicationCore.IRepositories;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class ShoppingBagRepository : RepositoryBase<ShoppingBagDAO>, IShoppingBagRepository
    {
        public ShoppingBagRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(ShoppingBag entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ShoppingBag entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ShoppingBag> FindByCondition(Expression<Func<ShoppingBag, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(ShoppingBag entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<ShoppingBag> IRepositoryBase<ShoppingBag>.FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
