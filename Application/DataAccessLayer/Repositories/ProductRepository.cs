using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;

namespace DataAccessLayer
{
    public class ProductRepository : RepositoryBase<ProductDAO>,IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> FindByCondition(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<Product> IRepositoryBase<Product>.FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
