using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;
using DataAccessLayer.DAOs;
using ApplicationCore.Mapping;

namespace DataAccessLayer.Repositories
{
    public class ShopRepository : RepositoryBase<ShopDAO>, IShopRepository
    {
        readonly BaseMapingManager _baseMapingManager;

        public ShopRepository(ApplicationContext context, BaseMapingManager baseMapingManager) : base(context)
        {
            _baseMapingManager = baseMapingManager;
        }

        public void Create(Shop entity)
        {
            var dto = _baseMapingManager.Map<Shop, ShopDAO>(entity);
            base.Create(dto);
        }

        public void Delete(Shop entity)
        {
            var dto = _baseMapingManager.Map<Shop, ShopDAO>(entity);
            base.Delete(dto);
        }

        public IQueryable<Shop> FindByCondition(Expression<Func<Shop, bool>> expression)
        {
            throw new NotImplementedException("DEPRECATED, USE FindAll and query it.");
        }

        public void Update(Shop entity)
        {
            var dto = _baseMapingManager.Map<Shop, ShopDAO>(entity);
            base.Update(dto);
        }

        IQueryable<Shop> IRepositoryBase<Shop>.FindAll()
        {
            return base.FindAll().Select(b => _baseMapingManager.Map<ShopDAO, Shop>(b));
        }
    }
}
