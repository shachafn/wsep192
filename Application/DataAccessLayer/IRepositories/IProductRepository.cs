using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IRepositories
{
    public interface IProductRepository : IRepositoryBase<ProductDAO>
    {
    }
}
