using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class UnitOfWork : IDisposable
    {
        ApplicationContext _context;

        private UserRepository _userRepository;
        private ShoppingBagRepository _shoppingBagRepository;
        private ShopProductRepository _shopProductRepository;
        private ShopRepository _shopRepository;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public UserRepository UserRepository
        {
            get
            {

                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public ShoppingBagRepository ShoppingBagRepository
        {
            get
            {

                if (_shoppingBagRepository == null)
                {
                    _shoppingBagRepository = new ShoppingBagRepository(_context);
                }
                return _shoppingBagRepository;
            }
        }

        public ShopProductRepository ShopProductRepository
        {
            get
            {

                if (_shopProductRepository == null)
                {
                    _shopProductRepository = new ShopProductRepository(_context);
                }
                return _shopProductRepository;
            }
        }

        public ShopRepository ShopRepository
        {
            get
            {

                if (_shopRepository == null)
                {
                    _shopRepository = new ShopRepository(_context);
                }
                return _shopRepository;
            }
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
