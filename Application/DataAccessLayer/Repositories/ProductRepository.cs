using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using DataAccessLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public class ProductRepository : RepositoryBase<ProductDAO>,IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
