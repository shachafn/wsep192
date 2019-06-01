using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IRepositories
{
    public interface IShoppingCartRepository : IRepositoryBase<ShoppingCartDAO>
    {
    }
}
