using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using ApplicationCore.IRepositories;
using System.Linq.Expressions;
using ApplicationCore.Mapping;

namespace DataAccessLayer.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCartDAO>, IShoppingCartRepository
    {
        readonly BaseMapingManager _baseMapingManager;

        public ShoppingCartRepository(ApplicationContext context, BaseMapingManager baseMapingManager) : base(context)
        {
            _baseMapingManager = baseMapingManager;
        }

        public void Create(ShoppingCart entity)
        {
            var dto = _baseMapingManager.Map<ShoppingCart, ShoppingCartDAO>(entity);
            base.Create(dto);
        }

        public void Delete(ShoppingCart entity)
        {
            var dto = _baseMapingManager.Map<ShoppingCart, ShoppingCartDAO>(entity);
            base.Delete(dto);
        }

        public override void DeleteAll()
        {
            base.Context.ShoppingCarts.RemoveRange(base.Context.ShoppingCarts);
        }

        public IQueryable<ShoppingCart> FindByCondition(Expression<Func<ShoppingCart, bool>> expression)
        {
            throw new NotImplementedException("DEPRECATED, USE FindAll and query it.");
        }

        public void Update(ShoppingCart entity)
        {
            var dto = _baseMapingManager.Map<ShoppingCart, ShoppingCartDAO>(entity);
            base.Update(dto);
        }

        IQueryable<ShoppingCart> IRepositoryBase<ShoppingCart>.FindAll()
        {
            return base.FindAll().Select(b => _baseMapingManager.Map<ShoppingCartDAO, ShoppingCart>(b));
        }

        ShoppingCart IRepositoryBase<ShoppingCart>.FindById(Guid id)
        {
            return _baseMapingManager.Map<ShoppingCartDAO, ShoppingCart>(base.FindById(id));
        }
    }
}
