namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface IUnitOfWork
    {
        IContext Context { get; }
        IShopRepository ShopRepository { get; }
        IBaseUserRepository BaseUserRepository { get; }
        IBagRepository BagRepository { get; }

        void ClearAll();
    }
}
