using DomainLayer.Data.Collections;

namespace DomainLayer.Data
{
    public static class DomainData
    {
        public static LoggedInUsersEntityCollection LoggedInUsersEntityCollection = new LoggedInUsersEntityCollection();
        public static ShopEntityCollection ShopsCollection = new ShopEntityCollection();
        public static AllUsersEntityCollection AllUsersCollection = new AllUsersEntityCollection();
        public static ShoppingBagsEntityCollection ShoppingBagsCollection = new ShoppingBagsEntityCollection();

    }
}
