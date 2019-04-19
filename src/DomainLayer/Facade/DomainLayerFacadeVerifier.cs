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
using DomainLayer.Data.Entitites.Users.States;

namespace DomainLayer.Facade
{
    public static class DomainLayerFacadeVerifier
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
            VerifyGuestUser(userGuid, new IllegalOperationException());
            VerifyString(username, new IllegalArgumentException());
            VerifyString(password, new IllegalArgumentException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked in UserDmain
        /// </constraints>
        public static void Login(Guid userGuid, string username, string password)
        {
            VerifyGuestUser(userGuid, new IllegalOperationException());
            VerifyLoginCredentials(username, password, new CredentialsMismatchException());
            VerifyString(username, new IllegalArgumentException());
            VerifyString(password, new IllegalArgumentException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// </constraints>
        public static void Logout(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid, new IllegalOperationException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void OpenShop(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid, new UserNotFoundException());
        }

        /////////// REDO CONSTRAINTS, CHANGED FROM CART TO BAG ////////////////
        public static void PurchaseBag(Guid userGuid)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
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
            VerifyGuestUser(userGuid, new IllegalOperationException());
            VerifyString(username, new IllegalArgumentException()); 
            VerifyString(password, new IllegalArgumentException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked.
        /// </constraints>
        public static void RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            VerifyRegisteredUser(userToRemoveGuid, new UserNotFoundException());
            VerifyNotOnlyOwnerOfAnActiveShop(userToRemoveGuid, new BrokenConstraintException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void ConnectToPaymentSystem(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid, new UserNotFoundException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void ConnectToSupplySystem(Guid userGuid)
        {
            VerifyLoggedInUser(userGuid, new UserNotFoundException());
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
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid, new ProductNotFoundException());
            VerifyIntGreaterThan0(quantity, new IllegalArgumentException());
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            VerifyShopProductDoesNotExist(cart, shopProductGuid, new ProductNotFoundException());
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
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
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
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            VerifyShopProductExistsInCart(cart, shopProductGuid, new ProductNotFoundException());
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
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            VerifyIntGreaterThan0(newAmount, new IllegalArgumentException());
            var cart = VerifyCartExistsAndCreateIfNeeded(userGuid, shopGuid);
            VerifyShopProductExistsInCart(cart, shopProductGuid, new ProductNotFoundException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. checked
        /// 9. checked
        /// 10. checked
        /// </constraints>
        public static void AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            VerifyString(name, new IllegalArgumentException());
            VerifyString(category, new IllegalArgumentException());
            VerifyDoubleGreaterThan0(price, new IllegalArgumentException());
            VerifyIntEqualOrGreaterThan0(quantity, new IllegalArgumentException());
            shop.VerifyCreatorOrOwnerOrManager(userGuid, new NoPriviligesException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public static void RemoveShopProduct(Guid userGuid, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid, new ProductNotFoundException());
            shop.VerifyCreatorOrOwnerOrManager(userGuid, new NoPriviligesException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// 8. checked
        /// 9. checked
        /// </constraints>
        public static void EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(productGuid, new ProductNotFoundException());
            VerifyDoubleGreaterThan0(newPrice, new IllegalArgumentException());
            VerifyIntEqualOrGreaterThan0(newQuantity, new IllegalArgumentException());
            shop.VerifyCreatorOrOwnerOrManager(userGuid, new NoPriviligesException());
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
            VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            VerifyString(productName, new IllegalArgumentException());
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
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            VerifyRegisteredUser(newManagaerGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyCreatorOrOwner(userGuid, new NoPriviligesException());
            shop.VerifyNotCreatorOrOwnerOrManager(newManagaerGuid, new BrokenConstraintException());
            shop.VerifyShopIsActive();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. ownerToRemove must not be the only owner of an active shop.
        /// 8. ownerToRemove must be an owner/manager of the shop.
        /// 9. ownerToRemove must have been appointed by the user with guid=userGuid
        /// </constraints>
        public static void CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyOwnerOrCreator(userGuid, new NoPriviligesException());
            VerifyRegisteredUser(ownerToRemoveGuid, new UserNotFoundException());
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
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            VerifyRegisteredUser(managerToRemoveGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyCreatorOrOwner(userGuid, new NoPriviligesException());
            shop.VerifyAppointedBy(managerToRemoveGuid, userGuid, new IllegalOperationException());
            shop.VerifyShopIsActive();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked
        /// 4. checked
        /// </constraints>
        public static void ChangeUserState(Guid userGuid, string newState)
        {
            var user = VerifyLoggedInUser(userGuid, new UserNotFoundException());
            VerifyString(newState, new IllegalArgumentException());
            VerifyStateString(newState, new IllegalArgumentException());
            VerifyIfChangeToAdminMustBeAdmin(user, newState, new IllegalOperationException());
        }

        #endregion

        #region Private Verifiers
        private static User VerifyLoggedInUser(Guid userGuid, ICloneableException<Exception> e)
        {
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            if (user == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = string.Format(Resources.EntityNotFound, "logged in user", userGuid) +
    $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
            return user;
        }


        private static Shop VerifyShopExists(Guid shopGuid, ICloneableException<Exception> e)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            if (shop == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = string.Format(Resources.EntityNotFound, "shop", shopGuid) +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
            return shop;
        }


        private static void VerifyGuestUser(Guid userGuid, ICloneableException<Exception> e)
        {
            if (!userGuid.Equals(GuestGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw e.Clone($"User with Guid - {userGuid} is not a guest." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private static void VerifyRegisteredUser(Guid userGuid, ICloneableException<Exception> e)
        {
            if (!DomainData.RegisteredUsersCollection.ContainsKey(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = string.Format(Resources.EntityNotFound, "registered user", userGuid) +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyString(string str, ICloneableException<Exception> e)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"String is null, empty or whitespace." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyDoubleGreaterThan0(double toCheck, ICloneableException<Exception> e)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyIntEqualOrGreaterThan0(int toCheck, ICloneableException<Exception> e)
        {
            if (toCheck < 0)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }


        private static void VerifyIntGreaterThan0(int toCheck, ICloneableException<Exception> e)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyNotOnlyOwnerOfAnActiveShop(Guid userGuid, ICloneableException<Exception> e)
        {
            var constraint = DomainData.ShopsCollection.Any(shop =>
            {
                var isCreator = shop.Creator.OwnerGuid.Equals(userGuid);
                var isOwner = shop.Owners.Any(owner => owner.OwnerGuid.Equals(userGuid));
                var shopIsActive = shop.ShopState.Equals(ShopStateEnum.Active);
                var isOnlyOwnerByCreator = isCreator && (shop.Owners.Count == 0);
                var isOnlyOwnerByOwner = shop.Creator.OwnerGuid.Equals(Guid.Empty) && (shop.Owners.Count == 0);

                return (shopIsActive && (isOnlyOwnerByCreator || isOnlyOwnerByOwner));
            });
            if (constraint)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {userGuid} is the only owner of an active shop." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
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

        private static void VerifyStateString(string newState, ICloneableException<Exception> e)
        {
            if (!(newState.Equals(AdminUserState.AdminUserStateString)
                  || newState.Equals(BuyerUserState.BuyerUserStateString)
                  || newState.Equals(SellerUserState.SellerUserStateString)))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"State string doesnt not match any state." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyLoginCredentials(string username, string password, ICloneableException<Exception> e)
        {
            if (!DomainData.RegisteredUsersCollection.Any(u => u.Username.ToLower().Equals(username.ToLower()) && u.CheckPass(password)))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Login Credentials does not match any registered user." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyIfChangeToAdminMustBeAdmin(User user, string newState, ICloneableException<Exception> e)
        {
            if (newState.Equals(AdminUserState.AdminUserStateString) && !user.IsAdmin)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Can't change state to admin state with the user not being an admin." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        public static void VerifyShopProductDoesNotExist(ShoppingCart cart, Guid shopProductGuid, ICloneableException<Exception> e)
        {
            var product = cart.PurchasedProducts.FirstOrDefault(prod => prod.Item1.Equals(shopProductGuid));
            if (product != null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Cannot add the same product with Guid - {shopProductGuid} to the cart of user" +
                    $" Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        public static void VerifyShopProductExistsInCart(ShoppingCart cart, Guid shopProductGuid, ICloneableException<Exception> e)
        {
            var product = cart.PurchasedProducts.FirstOrDefault(prod => prod.Item1.Equals(shopProductGuid));
            if (product == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"ShopProduct with Guid - {shopProductGuid} doesnt exist in the cart." +
                    $" Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        #endregion
    }
}
