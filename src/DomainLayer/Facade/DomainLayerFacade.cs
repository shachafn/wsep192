using DomainLayer.Data;
using DomainLayer.Domains;
using DomainLayer.Exceptions;
using System;

namespace DomainLayer.Facade
{
    public class DomainLayerFacade : IDomainLayerFacade
    {
        UserDomain UserDomain = UserDomain.Instance;

        #region Singleton Implementation
        private static IDomainLayerFacade instance = null;
        private static readonly object padlock = new object();
        public static IDomainLayerFacade Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DomainLayerFacade();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public bool Register(string username, string password)
        {
            return UserDomain.Register(username, password);
        }

        public Guid Login(Guid userGuid, string username, string password)
        {
            return UserDomain.Login(userGuid, username, password);
        }

        public bool Logout(Guid userGuid)
        {
            return UserDomain.LogoutUser(userGuid);
        }

        public Guid OpenShop(Guid userGuid)
        {
            return UserDomain.OpenShopForUser(userGuid);
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            return UserDomain.PurchaseCart(userGuid, shopGuid);
        }

        public bool Initialize(Guid userGuid, string username = null, string password = null)
        {
            if (!External_Services.ExternalServicesManager._paymentSystem.IsAvailable())
                return false;
            if (!External_Services.ExternalServicesManager._supplySystem.IsAvailable())
                return false;
            if ((DomainData.AllUsersCollection.Count == 0) && (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)))
                throw new BrokenConstraintException("Can't initialize system with no admin user when no new user information was provided.");

            if (Register(username, password))
                return true;

            return false;
        }

        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be admin.
        /// </constraints>
        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            if (!DomainData.LoggedInUsersEntityCollection.ContainsKey(userGuid))
                throw new UserNotFoundException($"No logged in user with Guid - {userGuid}");
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            return user.ConnectToPaymentSystem();
        }

        public bool ConnectToSupplySystem(Guid userGuid)
        {
            if (!DomainData.LoggedInUsersEntityCollection.ContainsKey(userGuid))
                throw new UserNotFoundException($"No logged in user with Guid - {userGuid}");
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            return user.ConnectToSupplySystem();
        }

        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. name must not be null or string.Empty
        /// 8. category must not be null or string.Empty
        /// 9. price must be above 0
        /// 10. quantity must be equal or greater than 0 (May not have any to sell at the moment).
        /// </constraints>
        public Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            if (!DomainData.LoggedInUsersEntityCollection.ContainsKey(userGuid))
                throw new UserNotFoundException($"No logged in user with Guid - {userGuid}");
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            user.AddShopProduct(shopGuid, name, category, price, quantity);
            return true;
        }


        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in shop.
        /// 8. newPrice must be greater than 0.
        /// 9. newQuantity must be equal or greater than 0 (May not have any to sell at the moment).
        /// </constraints>
        public bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            if (!DomainData.LoggedInUsersEntityCollection.ContainsKey(userGuid))
                throw new UserNotFoundException($"No logged in user with Guid - {userGuid}");
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            user.EditShopProduct(userGuid, shopGuid, productGuid, newPrice, newQuantity);
        }
    }
}
