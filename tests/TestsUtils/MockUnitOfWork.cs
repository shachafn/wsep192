using ApplicationCore.Interfaces.DataAccessLayer;

namespace TestsUtils
{
    public class MockUnitOfWork : IUnitOfWork
    {
        MockContext _context;
        public MockUnitOfWork(MockContext context)
        {
            _context = context;

        }
        public IContext Context => _context;

        public IShopRepository ShopRepository { get; set; } = new MockShopRepository();

        public IBaseUserRepository BaseUserRepository { get; set; } = new MockBaseUserRepository();

        public IBagRepository BagRepository { get; set; } = new MockBagRepository();

        public void ClearAll()
        {
            ShopRepository.Clear();
            BaseUserRepository.Clear();
            BagRepository.Clear();
        }
    }
}
