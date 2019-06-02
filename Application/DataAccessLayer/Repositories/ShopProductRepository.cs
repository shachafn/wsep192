using ApplicationCore.Entitites;
using ApplicationCore.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class ShopProductRepository : RepositoryBase<ShopProductDAO>, IShopProductRepository
    {
        public ShopProductRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(ShopProduct entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ShopProduct entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ShopProduct> FindByCondition(Expression<Func<ShopProduct, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(ShopProduct entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<ShopProduct> IRepositoryBase<ShopProduct>.FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
