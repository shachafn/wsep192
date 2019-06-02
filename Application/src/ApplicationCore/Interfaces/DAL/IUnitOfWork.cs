using ApplicationCore.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces.DAL
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IShoppingBagRepository ShoppingBagRepository { get; }
        IShopRepository ShopRepository { get; }
    }
}
