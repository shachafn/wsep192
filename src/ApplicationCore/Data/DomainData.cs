using ApplicationCore.Data.Collections;

namespace ApplicationCore.Data
{
    public static class DomainData
    {
        public static LoggedInUsersEntityCollection LoggedInUsersEntityCollection = new LoggedInUsersEntityCollection();
        public static ShopsEntityCollection ShopsCollection = new ShopsEntityCollection();
        public static RegisteredUsersEntityCollection RegisteredUsersCollection = new RegisteredUsersEntityCollection();
        public static ShoppingBagsEntityCollection ShoppingBagsCollection = new ShoppingBagsEntityCollection();
        public static GuestsEntityCollection GuestsCollection = new GuestsEntityCollection();

        public static void ClearAll()
        {
            LoggedInUsersEntityCollection.Clear();
            ShopsCollection.Clear();
            RegisteredUsersCollection.Clear();
            ShoppingBagsCollection.Clear();
            GuestsCollection.Clear();
        }
    }
}
