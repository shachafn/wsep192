using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using ApplicationCore.IRepositories;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.Mapping;

namespace DataAccessLayer.Repositories
{
    public class ShoppingBagRepository : RepositoryBase<ShoppingBagDAO>, IShoppingBagRepository
    {
        readonly BaseMapingManager _baseMapingManager;

        public ShoppingBagRepository(ApplicationContext context, BaseMapingManager baseMapingManager) : base(context)
        {
            _baseMapingManager = baseMapingManager;
        }

        public void Create(ShoppingBag entity)
        {
            var dto = _baseMapingManager.Map<ShoppingBag, ShoppingBagDAO>(entity);
            base.Create(dto);
        }

        public void Delete(ShoppingBag entity)
        {
            var dto = _baseMapingManager.Map<ShoppingBag, ShoppingBagDAO>(entity);
            base.Delete(dto);
        }

        public override void DeleteAll()
        {
            base.Context.ShoppingBags.RemoveRange(base.Context.ShoppingBags);
        }

        public IQueryable<ShoppingBag> FindByCondition(Expression<Func<ShoppingBag, bool>> expression)
        {
            throw new NotImplementedException("DEPRECATED, USE FindAll and query it.");
        }

        public void Update(ShoppingBag entity)
        {
            var dto = _baseMapingManager.Map<ShoppingBag, ShoppingBagDAO>(entity);
            base.Update(dto);
        }

        IQueryable<ShoppingBag> IRepositoryBase<ShoppingBag>.FindAll()
        {
            return base.FindAll().Select(b => _baseMapingManager.Map<ShoppingBagDAO, ShoppingBag>(b));
        }

        ShoppingBag IRepositoryBase<ShoppingBag>.FindById(Guid id)
        {
            return _baseMapingManager.Map<ShoppingBagDAO, ShoppingBag>(base.FindById(id));
        }
    }
}
