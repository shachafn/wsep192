using ApplicationCore.Entitites;
using ApplicationCore.IRepositories;
using ApplicationCore.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class ShopProductRepository : RepositoryBase<ShopProductDAO>, IShopProductRepository
    {
        readonly BaseMapingManager _baseMapingManager;

        public ShopProductRepository(ApplicationContext context, BaseMapingManager baseMapingManager) : base(context)
        {
            _baseMapingManager = baseMapingManager;
        }

        public void Create(ShopProduct entity)
        {
            var dto = _baseMapingManager.Map<ShopProduct, ShopProductDAO>(entity);
            base.Create(dto);
        }

        public void Delete(ShopProduct entity)
        {
            var dto = _baseMapingManager.Map<ShopProduct, ShopProductDAO>(entity);
            base.Delete(dto);
        }

        public override void DeleteAll()
        {
            base.Context.ShopProducts.RemoveRange(base.Context.ShopProducts);
        }

        public IQueryable<ShopProduct> FindByCondition(Expression<Func<ShopProduct, bool>> expression)
        {
            throw new NotImplementedException("DEPRECATED, USE FindAll and query it.");
        }

        public void Update(ShopProduct entity)
        {
            var dto = _baseMapingManager.Map<ShopProduct, ShopProductDAO>(entity);
            base.Update(dto);
        }

        IQueryable<ShopProduct> IRepositoryBase<ShopProduct>.FindAll()
        {
            return base.FindAll().Select(b => _baseMapingManager.Map<ShopProductDAO, ShopProduct>(b));
        }
    }
}
