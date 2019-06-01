using ApplicationCore.Entitites;
using DataAccessLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class ShopRepositorycs : RepositoryBase<Shop>, IShopRepository
    {
        public ShopRepositorycs(RepositoryContext context) : base(context)
        {
        }
    }
}
