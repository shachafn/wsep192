using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        public IContext Context { get; set; }

        public UnitOfWork(IContext context)
        {
            Context = context;
        }

        IShopRepository _shopRepository;
        public IShopRepository ShopRepository
        {
            get
            {
                if (_shopRepository == null)
                    _shopRepository = new ShopRepository(Context);
                return _shopRepository;
            }
        }

        IBaseUserRepository _baseUserRepository;
        public IBaseUserRepository BaseUserRepository
        {
            get
            {
                if (_baseUserRepository == null)
                    _baseUserRepository = new BaseUserRepository(Context);
                return _baseUserRepository;
            }
        }

        IBagRepository _bagRepository;
        public IBagRepository BagRepository
        {
            get
            {
                if (_bagRepository == null)
                    _bagRepository = new BagRepository(Context);
                return _bagRepository;
            }
        }

        public void ClearAll()
        {
            BaseUserRepository.Clear();
            ShopRepository.Clear();
            BagRepository.Clear();
        }

        public void CreateDatabase()
        {
            ((MongoDbContext)Context).GetMongoDatabase();
            ((MongoDbContext)Context).GetMongoDatabase().CreateCollection(nameof(Shop));
            ((MongoDbContext)Context).GetMongoDatabase().CreateCollection(nameof(BaseUser));
            ((MongoDbContext)Context).GetMongoDatabase().CreateCollection(nameof(ShoppingBag));
        }
    }
}
