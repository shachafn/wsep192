using DomainLayer.Data;
using DomainLayer.Data.Entitites;
using DomainLayer.Exceptions;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using static DomainLayer.Data.Entitites.Shop;
using System.Reflection;

namespace DomainLayer.Facade
{
    public class DomainLayerFacadeVerifier
    {
        public static Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");

        #region VerifyMe
        public static void VerifyMe(MethodBase methodInfo, params object[] parameters)
        {
            MethodInfo method;
            try
            {
                var type = (typeof(DomainLayerFacadeVerifier));
                method = type.GetMethod(methodInfo.Name, methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
                if (method == null)
                    throw new VerifierReflectionNotFound("Couldnt call function", new Exception());
            }
            catch (Exception ex)
            {
                throw new VerifierReflectionNotFound("Couldnt call function", ex);
            }
            try
            {
                method.Invoke(typeof(DomainLayerFacadeVerifier), parameters);
            }
            catch (Exception ex) 
            {
                var is1 = ex is AmbiguousMatchException;
                var is2 = ex is ArgumentNullException;
                var is3 = ex is ArgumentException;
                var is4 = ex is TargetException;
                var is5 = ex is TargetParameterCountException;
                var is6 = ex is MethodAccessException;
                var is7 = ex is InvalidOperationException;
                var is8 = ex is NotSupportedException;
                if (is1 || is2 || is3 || is4 || is5 || is6  || is7 || is8)
                    throw new VerifierReflectionNotFound("Couldnt verify.", ex);
                else
                    throw ex.InnerException;
            }
        }
        #endregion

        #region Facade Verifiers
        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked in UserDomain
        /// </constraints>
        public static void Register(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid);
            VerifyString(username);
            VerifyString(password);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked in UserDmain
        /// </constraints>
        public static void Login(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid);
            VerifyString(username);
            VerifyString(password);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// </constraints>
        public static void Logout(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void OpenShop(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked
        /// 5. checked. 
        /// 6. User must have at least one item in cart.
        /// </constraints>
        public static void PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
        }


        /// <constraints>
        /// 1. checked
        /// 2. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If an admin user exists - username and password must match it.
        /// 3. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If no admin user exists - username and password will be used to create one.
        /// 4. checked
        /// 5. if any external service is unavailable - return false
        /// </constraints>
        public static void Initialize(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid);
            VerifyString(username); 
            VerifyString(password);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void ConnectToPaymentSystem(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void ConnectToSupplySystem(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid);
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
        public static void AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyString(name);
            VerifyString(category);
            VerifyDoubleGreaterThan0(price);
            VerifyIntEqualOrGreaterThan0(quantity);
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
        public static void EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(productGuid);
            VerifyDoubleGreaterThan0(newPrice);
            VerifyIntEqualOrGreaterThan0(newQuantity);
            user.EditShopProduct(shopGuid, productGuid, newPrice, newQuantity);
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
        public static void RemoveShopProduct(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid);
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
        public static void AddProductToShoppingCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid);
            VerifyIntGreaterThan0(quantity);
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
        public static void AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(newManagaerGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
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
        public static void CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(ownerToRemoveGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
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
        public static void EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyIntGreaterThan0(newAmount);
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            cart.VerifyShopProductExists(shopProductGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public static void RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            cart.VerifyShopProductExists(shopProductGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// </constraints>
        public static void GetAllProductsInCart(Guid userGuid, Guid shopGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked.
        /// </constraints>
        public static void RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(userToRemoveGuid);
            VerifyNotOnlyOwnerOfAnActiveShop(userToRemoveGuid); 
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. GUEST NOT IMPLEMENTED ------checked by design by state pattern - User must be in buyer/guest state.
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public static void SearchProduct(Guid userGuid, Guid shopGuid, string productName)
        {
            VerifyLoggedInUser(userGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
            VerifyString(productName);
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
        public static void RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid);
            VerifyRegisteredUser(managerToRemoveGuid);
            var shop = VerifyShopExists(shopGuid);
            shop.VerifyShopIsActive();
        }

        #endregion

        #region Private Verifiers
        private static User VerifyLoggedInUser(Guid userGuid)
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


        private static Shop VerifyShopExists(Guid shopGuid)
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


        private static void VerifyGuestUser(Guid userGuid)
        {
            if (!userGuid.Equals(GuestGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new UserNotFoundException($"User with Guid - {userGuid} is not a guest." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private static void VerifyRegisteredUser(Guid userGuid)
        {
            if (!DomainData.AllUsersCollection.ContainsKey(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new UserNotFoundException(string.Format(Resources.EntityNotFound, "registered user", userGuid) +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private static void VerifyString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"String is null, empty or whitespace." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private static void VerifyDoubleGreaterThan0(double toCheck)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private static void VerifyIntEqualOrGreaterThan0(int toCheck)
        {
            if (toCheck < 0)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }


        private static void VerifyIntGreaterThan0(int toCheck)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private static void VerifyNotOnlyOwnerOfAnActiveShop(Guid userGuid)
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

        private static ShoppingCart VerifyCartExistsAndCreateIfNeeded(Guid userGuid, Guid shopGuid)
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
        #endregion
    }
}
