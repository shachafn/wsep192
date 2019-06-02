using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;

namespace DataAccessLayer.Repositories
{
    public class ShopRepository : RepositoryBase<Shop>, IShopRepository
    {
        public ShopRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
