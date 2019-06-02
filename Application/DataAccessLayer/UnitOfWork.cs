using ApplicationCore.Interfaces.DAL;
using ApplicationCore.IRepositories;
using ApplicationCore.Mapping;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        ApplicationContext _context;

        public IUserRepository UserRepository { get; set; }
        public IShoppingBagRepository ShoppingBagRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }

        public UnitOfWork(ApplicationContext context, BaseMapingManager baseMapingManager)
        {
            _context = context;
            UserRepository = new UserRepository(context, baseMapingManager);
            ShoppingBagRepository = new ShoppingBagRepository(context, baseMapingManager);
            ShopRepository = new ShopRepository(context, baseMapingManager); ;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
