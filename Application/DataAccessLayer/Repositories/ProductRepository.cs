using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;
using ApplicationCore.Mapping;

namespace DataAccessLayer
{
    public class ProductRepository : RepositoryBase<ProductDAO>,IProductRepository
    {
        readonly BaseMapingManager _baseMapingManager;

        public ProductRepository(ApplicationContext context, BaseMapingManager baseMapingManager) : base(context)
        {
            _baseMapingManager = baseMapingManager;
        }

        public void Create(Product entity)
        {
            var dto = _baseMapingManager.Map<Product, ProductDAO>(entity);
            base.Create(dto);
        }

        public void Delete(Product entity)
        {
            var dto = _baseMapingManager.Map<Product, ProductDAO>(entity);
            base.Delete(dto);
        }

        public override void DeleteAll()
        {
            base.Context.Products.RemoveRange(base.Context.Products);
        }

        public IQueryable<Product> FindByCondition(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException("DEPRECATED, USE FindAll and query it.");
        }

        public void Update(Product entity)
        {
            var dto = _baseMapingManager.Map<Product, ProductDAO>(entity);
            base.Update(dto);
        }

        IQueryable<Product> IRepositoryBase<Product>.FindAll()
        {
            return base.FindAll().Select(b => _baseMapingManager.Map<ProductDAO, Product>(b));
        }
    }
}
