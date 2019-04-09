using DomainLayer.Data;
using DomainLayer.Data.Entitites;
using DomainLayer.Domains;
using DomainLayer.Exceptions;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static DomainLayer.Data.Entitites.Shop;

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

        public Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");


        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked in UserDomain
        /// </constraints>
        public bool Register(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid);
            VerifyString(username);
            VerifyString(password);
            return UserDomain.Register(username, password);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked in UserDmain
        /// </constraints>
        public Guid Login(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid);
            VerifyString(username);
            VerifyString(password);
            return UserDomain.Login(userGuid, username, password);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// </constraints>
        public bool Logout(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid);
            return UserDomain.LogoutUser(userGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public Guid OpenShop(Guid userGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            return user.OpenShop();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked
        /// 5. checked. 
        /// 6. User must have at least one item in cart.
        /// </constraints>
        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            // Need to actually pay for products
            // if success clear cart
            return cart.PurchaseCart(user); //Not implemented
        }

        private ShoppingCart VerifyCartExistsAndCreateIfNeeded(Guid userGuid, Guid shopGuid)
        {
            var userBag = DomainData.ShoppingBagsCollection.FirstOrDefault(bag => bag.UserGuid.Equals(userGuid));
            var userCart = userBag.ShoppingCarts.FirstOrDefault(cart => cart.ShopGuid.Equals(shopGuid));
            if (userCart == null)
            {
                userCart = new ShoppingCart(userGuid, shopGuid);
                userBag.ShoppingCarts.Add(userCart);
            }
            return userCart;
        }

        /// <constraints>
        /// 1. checked
        /// 2. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If an admin user exists - username and password must match it.
        /// 3. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If no admin user exists - username and password will be used to create one.
        /// 4. checked
        /// 5. if any external service is unavailable - return false
        /// </constraints>
        public bool Initialize(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid);
            VerifyString(username); // If we call the register function on this class - this will be checked there
            VerifyString(password); // If we call the register function on this class - this will be checked there
            // check for admin user exists - need to add IsAdmin to user
            // register if needed

            if (!External_Services.ExternalServicesManager._paymentSystem.IsAvailable())
                return false;
            if (!External_Services.ExternalServicesManager._supplySystem.IsAvailable())
                return false;

            throw new NotImplementedException();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public bool ConnectToPaymentSystem(Guid userGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            return user.ConnectToPaymentSystem();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public bool ConnectToSupplySystem(Guid userGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            return user.ConnectToSupplySystem();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. Will be checked in Shop class - User must be creator, or an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. checked
        /// 9. checked
        /// 10. checked
        /// </constraints>
        public Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyString(name);
            VerifyString(category);
            VerifyDoubleGreaterThan0(price);
            VerifyIntEqualOrGreaterThan0(quantity);
            return user.AddShopProduct(shopGuid, name, category, price, quantity);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. Will be checked in Shop class - User must be creator, an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. checked
        /// 9. checked
        /// </constraints>
        public bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(productGuid);
            VerifyDoubleGreaterThan0(newPrice);
            VerifyIntEqualOrGreaterThan0(newQuantity);
            user.EditShopProduct(shopGuid, productGuid, newPrice, newQuantity);
            return true;
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. will be checked in Shop class - User must be creator, an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public bool RemoveShopProduct(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid);
            return user.RemoveShopProduct(shopGuid, shopProductGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. User must not have the item in the cart. (For edit - use EditProductInCart)
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. quantity must be greater than 0
        /// </constraints>
        public bool AddProductToShoppingCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid);
            VerifyIntGreaterThan0(quantity);
            return user.AddProductToShoppingCart(shopGuid, shopProductGuid, quantity);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. Will be checked in Shop class - User must be an owner of the shop.
        /// 5. checked
        /// 6. checked
        /// 7. checked.
        /// 8. Will be checked in Shop class - newManagaerGuid must not be the creator of the shop, or one of the owners/managers.
        /// </constraints>
        public bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(newManagaerGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            return user.AddShopManager(shopGuid, newManagaerGuid, priviliges);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. Will be checked in Shop class - User must be an owner of the shop.
        /// 5. checked
        /// 6. checked
        /// 7. Will be checked in Shop class - ownerToRemove must not be the creator of the shop.
        /// 8. checked
        /// 9. Will be checked in Shop class - ownerToRemove must have been appointed by the user with guid=userGuid
        /// </constraints>
        public bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(ownerToRemoveGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            return user.CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. checked
        /// </constraints>
        public bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyIntGreaterThan0(newAmount);
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            cart.VerifyShopProductExists(shopProductGuid);
            return user.EditProductInCart(shopGuid, shopProductGuid, newAmount);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            cart.VerifyShopProductExists(shopProductGuid);
            return user.RemoveProductFromCart(shopGuid, shopProductGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// </constraints>
        public ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            return user.GetAllProductsInCart(shopGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked.
        /// </constraints>
        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(userToRemoveGuid);
            VerifyNotOnlyOwnerOfAnActiveShop(userToRemoveGuid);
            //return user.RemoveUser(userToRemoveGuid); // not implemented
            throw new NotImplementedException();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. GUEST NOT IMPLEMENTED ------checked by design by state pattern - User must be in buyer/guest state.
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyString(productName);
            return shop.SearchProduct(productName);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. Will be checked in SHop class - newManagaerGuid must not be the creator of the shop, or one of the owners/managers.
        /// </constraints>
        public bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(managerToRemoveGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            return user.RemoveShopManager(shopGuid, managerToRemoveGuid);
        }

        #region Verifiers
        /// <summary>
        /// Verifies that the user is logged in.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>A user object of this logged in user, if did not throw exception.</returns>
        private User VerifyLoggedInUser(Guid userGuid)
        {
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            if (user == null)
            {
                StackTrace stackTrace = new StackTrace();
                throw new UserNotFoundException(string.Format(Resources.EntityNotFound, "logged in user", userGuid) +
    $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
            return user;
        }

        /// <summary>
        /// Verifies that the shop exists.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>A shop object of this shop, if did not throw exception.</returns>
        private Shop VerifyShopExists(Guid shopGuid)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            if (shop == null)
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopNotFoundException(string.Format(Resources.EntityNotFound, "shop", shopGuid) +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
            return shop;
        }

        private void VerifyGuestUser(Guid userGuid)
        {
            if (!userGuid.Equals(GuestGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new UserNotFoundException($"User with Guid - {userGuid} is not a guest." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private void VerifyRegisteredUser(Guid userGuid)
        {
            if (!DomainData.AllUsersCollection.ContainsKey(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new UserNotFoundException(string.Format(Resources.EntityNotFound, "registered user", userGuid) +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        
        private void VerifyString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"String is null, empty or whitespace." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private void VerifyDoubleGreaterThan0(double toCheck)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private void VerifyIntEqualOrGreaterThan0(int toCheck)
        {
            if (toCheck < 0)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }


        private void VerifyIntGreaterThan0(int toCheck)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private void VerifyNotOnlyOwnerOfAnActiveShop(Guid userGuid)
        {
            var constraint = DomainData.ShopsCollection.Any(shop =>
            {
                var isCreator = shop.Creator.OwnerGuid.Equals(userGuid);
                var isOwner = shop.Owners.Any(owner => owner.OwnerGuid.Equals(userGuid));
                var shopIsActive = shop.ShopState.Equals(ShopStateEnum.Active);
                var isOnlyOwnerByCreator = isCreator && (shop.Owners.Count == 0);
                var isOnlyOwnerByOwner = shop.Creator.Guid.Equals(Guid.Empty) && (shop.Owners.Count == 0);

                return (shopIsActive && (isOnlyOwnerByCreator || isOnlyOwnerByOwner));
            });
            if (constraint)
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"User with Guid - {userGuid} is the only owner of an active shop." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        #endregion
    }
}
