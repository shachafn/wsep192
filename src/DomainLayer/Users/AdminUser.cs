using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using static ApplicationCore.Entitites.Shop;

namespace DomainLayer.Users
{
    public class AdminUser : RegisteredUser, IAdminUser
    {
        public AdminUser(BaseUser baseUser, IUnitOfWork unitOfWork, ShopDomain shopDomain) : base(baseUser, unitOfWork, shopDomain)
        {
        }

        /// <constraints>
        /// 4. UserToRemove must not be the only owner of an active shop. verfied by facade
        /// 
        /// </constraints>
        public bool RemoveUser(Guid userToRemoveGuid)
        {

            //if the user is an shop owner\manager Clear shops from the user as creator or other owner
            // and Clear shops from owners or managers appointed by this user 
            ICollection<Shop> shopsOwned = GetShopsOwnedByUser(userToRemoveGuid);
            foreach (Shop shop in shopsOwned)
            {
                //TODO-FIX shopDomain.RemoveOwner(userToRemoveGuid);
            }
            //Clear user's bag if exsits from the list 
            //TODO-FIX DomainData.ShoppingBagsCollection.Remove(userToRemoveGuid);
            //Clear shop products--->>???
            //Clear user registration
            //TODO-FIX DomainData.RegisteredUsersCollection.Remove(userToRemoveGuid);
            //Clear user from logged in - Maybe block this operation if the user is logged in, or its a real pain to cut him off.
            DomainData.LoggedInUsersEntityCollection.Remove(userToRemoveGuid);
            return true;
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory()
        {
            return null;//TODO-FIX return DomainData.ShopsCollection.SelectMany(shop => shop.GetPurchaseHistory(Guid)).ToList();
        }

        private ICollection<Shop> GetShopsOwnedByUser(Guid userToRemoveGuid)
        {
            return _unitOfWork.ShopRepository.Query().Where
                (shop => shop.ShopState.Equals(ShopStateEnum.Active) &&
                    (shop.Owners.Any(sOwner => sOwner.OwnerGuid.Equals(userToRemoveGuid))
                    ||
                    (shop.Creator.OwnerGuid.Equals(userToRemoveGuid)))).ToList();
        }

        public bool ConnectToPaymentSystem()
        {
            return true;
            //return External_Services.ExternalServicesManager._paymentSystem.IsAvailable();
        }

        public bool ConnectToSupplySystem()
        {
            return true;
            //return External_Services.ExternalServicesManager._supplySystem.IsAvailable();
        }
        
    }
}
