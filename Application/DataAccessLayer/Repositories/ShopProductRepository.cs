using ApplicationCore.Entitites;
using DataAccessLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class ShopProductRepository : RepositoryBase<ShopProduct>, IShopProductRepository
    {
        public ShopProductRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
