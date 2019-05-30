using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ProductRepository : RepositoryBase<Product>,IProductRepository
    {
        public ProductRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
