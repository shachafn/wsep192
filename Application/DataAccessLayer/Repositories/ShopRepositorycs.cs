using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories
{
    class ShopRepositorycs : RepositoryBase<Shop>, IShopRepository
    {
        public ShopRepositorycs(RepositoryContext context) : base(context)
        {
        }
    }
}
