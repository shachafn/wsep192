using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;
using DataAccessLayer.DAOs;

namespace DataAccessLayer.Repositories
{
    public class ShopRepository : RepositoryBase<ShopDAO>, IShopRepository
    {
        public ShopRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(Shop entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Shop entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Shop> FindByCondition(Expression<Func<Shop, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(Shop entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<Shop> IRepositoryBase<Shop>.FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
