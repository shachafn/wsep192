using ApplicationCore.Data.Collections;

namespace ApplicationCore.Data
{
    public static class DomainData
    {
        public static LoggedInUsersEntityCollection LoggedInUsersEntityCollection = new LoggedInUsersEntityCollection();
        public static GuestsEntityCollection GuestsCollection = new GuestsEntityCollection();
        public static GuestsShoppingBagsEntityCollection GuestsBagsCollection = new GuestsShoppingBagsEntityCollection();

        public static void ClearAll()
        {
            LoggedInUsersEntityCollection.Clear();
            GuestsCollection.Clear();
            GuestsBagsCollection.Clear();
        }
    }
}
