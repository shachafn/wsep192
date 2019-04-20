using DomainLayer.Data.Collections;

namespace DomainLayer.Data
{
    public static class DomainData
    {
        public static LoggedInUsersEntityCollection LoggedInUsersEntityCollection = new LoggedInUsersEntityCollection();
        public static ShopEntityCollection ShopsCollection = new ShopEntityCollection();
        public static RegisteredUsersEntityCollection RegisteredUsersCollection = new RegisteredUsersEntityCollection();
        public static ShoppingBagsEntityCollection ShoppingBagsCollection = new ShoppingBagsEntityCollection();

        public static void ClearAll()
        {
            LoggedInUsersEntityCollection.Clear();
            ShopsCollection.Clear();
            RegisteredUsersCollection.Clear();
            ShoppingBagsCollection.Clear();
        }
    }
}
