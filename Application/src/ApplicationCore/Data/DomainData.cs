using ApplicationCore.Data.Collections;
using ApplicationCore.Entities.Users;
using ApplicationCore.IRepositories;

namespace ApplicationCore.Data
{
    public static class DomainData
    {
        public static IRepositoryBase<IUser>          LoggedInUsersEntityCollection =  new LoggedInUsersRepository();
        public static IShopRepository                 ShopsCollection;
        public static IUserRepository                 RegisteredUsersCollection;
        public static IShoppingBagRepository          ShoppingBagsCollection;
        public static IRepositoryBase<IUser>          GuestsCollection              =  new GuestRepository();

        public static void ClearAll()
        {
            //LoggedInUsersEntityCollection.Clear();
            //ShopsCollection.Clear();
            //RegisteredUsersCollection.Clear();
            //ShoppingBagsCollection.Clear();
            //GuestsCollection.Clear();
        }
    }
}
