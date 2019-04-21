﻿using DomainLayer.Data;
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
using DomainLayer.ExposedClasses;

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
        public static void Register(UserIdentifier userIdentifier, string username, string password)
        {
            VerifyGuestUser(userIdentifier, new IllegalOperationException());
            VerifyString(username, new IllegalArgumentException());
            VerifyString(password, new IllegalArgumentException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked in UserDmain
        /// </constraints>
        public static void Login(UserIdentifier userIdentifier, string username, string password)
        {
            VerifyGuestUser(userIdentifier, new IllegalOperationException());
            VerifyLoginCredentials(username, password, new CredentialsMismatchException());
            VerifyString(username, new IllegalArgumentException());
            VerifyString(password, new IllegalArgumentException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// </constraints>
        public static void Logout(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new IllegalOperationException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void OpenShop(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        /////////// REDO CONSTRAINTS, CHANGED FROM CART TO BAG ////////////////
        public static void PurchaseBag(UserIdentifier userIdentifier)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If an admin user exists - username and password must match it.
        /// 3. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If no admin user exists - username and password will be used to create one.
        /// 4. checked
        /// 5. if any external service is unavailable - return false
        /// </constraints>
        public static void Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            VerifyGuestUser(userIdentifier, new IllegalOperationException());
            VerifyString(username, new IllegalArgumentException()); 
            VerifyString(password, new IllegalArgumentException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 4. checked.
        /// </constraints>
        public static void RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyRegisteredUser(userToRemoveGuid, new UserNotFoundException());
            VerifyNotOnlyOwnerOfAnActiveShop(userToRemoveGuid, new BrokenConstraintException());
            VerifyNotOnlyAdmin(userToRemoveGuid, new BrokenConstraintException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public static void ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
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
        public static void AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            VerifyString(name, new IllegalArgumentException());
            VerifyString(category, new IllegalArgumentException());
            VerifyDoubleGreaterThan0(price, new IllegalArgumentException());
            VerifyIntEqualOrGreaterThan0(quantity, new IllegalArgumentException());
            shop.VerifyCreatorOrOwnerOrManager(userIdentifier.Guid, new NoPriviligesException());
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
        public static void RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid, new ProductNotFoundException());
            shop.VerifyCreatorOrOwnerOrManager(userIdentifier.Guid, new NoPriviligesException());
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
        public static void EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(productGuid, new ProductNotFoundException());
            VerifyDoubleGreaterThan0(newPrice, new IllegalArgumentException());
            VerifyIntEqualOrGreaterThan0(newQuantity, new IllegalArgumentException());
            shop.VerifyCreatorOrOwnerOrManager(userIdentifier.Guid, new NoPriviligesException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. GUEST NOT IMPLEMENTED ------checked by design by state pattern - User must be in buyer/guest state.
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public static void SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            VerifyString(searchType, new IllegalArgumentException());
            VerifySearchString(searchType, new IllegalArgumentException());
            VerifySearchInput(toMatch, new IllegalArgumentException());
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
        public static void AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyShopProductExists(shopProductGuid, new ProductNotFoundException());
            VerifyIntGreaterThan0(quantity, new IllegalArgumentException());
            var cart = GetCartExistsAndCreateIfNeeded(userIdentifier, shopGuid);
            VerifyShopProductDoesNotExist(cart, shopProductGuid, new ProductNotFoundException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// </constraints>
        public static void GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            GetCartExistsAndCreateIfNeeded(userIdentifier, shopGuid);
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// 5. checked
        /// 6. checked
        /// 7. checked
        /// </constraints>
        public static void RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            var cart = GetCartExistsAndCreateIfNeeded(userIdentifier, shopGuid);
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
        public static void EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            VerifyIntGreaterThan0(newAmount, new IllegalArgumentException());
            var cart = GetCartExistsAndCreateIfNeeded(userIdentifier, shopGuid);
            VerifyShopProductExistsInCart(cart, shopProductGuid, new ProductNotFoundException());
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
        public static void AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyRegisteredUser(newManagaerGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyCreatorOrOwner(userIdentifier.Guid, new NoPriviligesException());
            shop.VerifyNotCreatorOrOwnerOrManager(newManagaerGuid, new BrokenConstraintException());
            shop.VerifyShopIsActive();
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
        public static void AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newOwnerGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyRegisteredUser(newOwnerGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyCreatorOrOwner(userIdentifier.Guid, new NoPriviligesException());
            shop.VerifyNotCreatorOrOwnerOrManager(newOwnerGuid, new BrokenConstraintException());
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
        /// 9. ownerToRemove must have been appointed by the user with guid=userIdentifier
        /// </constraints>
        public static void CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyOwnerOrCreator(userIdentifier.Guid, new NoPriviligesException());
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
        public static void RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyRegisteredUser(managerToRemoveGuid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyCreatorOrOwner(userIdentifier.Guid, new NoPriviligesException());
            shop.VerifyAppointedBy(managerToRemoveGuid, userIdentifier.Guid, new IllegalOperationException());
            shop.VerifyShopIsActive();
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked
        /// 4. checked
        /// </constraints>
        public static void ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyString(newState, new IllegalArgumentException());
            VerifyStateString(newState, new IllegalArgumentException());
            VerifyIfChangeToAdminMustBeAdmin(user, newState, new IllegalOperationException());
        }

        #endregion

        #region Private Verifiers
        private static RegisteredUser VerifyLoggedInUser(Guid userGuid, ICloneableException<Exception> e)
        {
            //Could equivalently check for IsGuest, but we also want to return the user.
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


        private static void VerifyGuestUser(UserIdentifier userIdentifier, ICloneableException<Exception> e)
        {
            if (!userIdentifier.IsGuest)
            {
                StackTrace stackTrace = new StackTrace();
                throw e.Clone($"User with Guid - {userIdentifier} is not a guest." +
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

        private static ShoppingCart GetCartExistsAndCreateIfNeeded(UserIdentifier userIdentifier, Guid shopGuid)
        {
            ShoppingBag bag = null;
            if (!DomainData.ShoppingBagsCollection.ContainsKey(userIdentifier.Guid))
            {
                bag = new ShoppingBag(userIdentifier.Guid);
                DomainData.ShoppingBagsCollection.Add(userIdentifier.Guid, bag);
            }
            else
                bag = DomainData.ShoppingBagsCollection[userIdentifier.Guid];

            ShoppingCart cart = null;
            if (!bag.ShoppingCarts.Any(c => c.ShopGuid.Equals(shopGuid)))
            {
                cart = new ShoppingCart(userIdentifier.Guid, shopGuid);
                bag.ShoppingCarts.Add(cart);
            }
            return bag.ShoppingCarts.First(c => c.ShopGuid.Equals(shopGuid));
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

        private static void VerifySearchString(string searchType, ICloneableException<Exception> e)
        {
            if (!(searchType.Equals("Name")
                  || searchType.Equals("Category")
                  || searchType.Equals("Keywords")))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Search Type string doesnt not match any search type." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifySearchInput(ICollection<string> toMatch, ICloneableException<Exception> e)
        {
            var isEmpty = toMatch.Count == 0;
            var validStrings = toMatch.All(s => !string.IsNullOrWhiteSpace(s));
            if (isEmpty || !validStrings)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Search must have input, input must be valid," +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private static void VerifyNotOnlyAdmin(Guid userToRemoveGuid, ICloneableException<Exception> e)
        {
            var admins = DomainData.RegisteredUsersCollection.Where(u => u.IsAdmin).ToList();
            if (admins.Count == 1 && admins.First().Guid.Equals(userToRemoveGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with guid - {userToRemoveGuid} is the only admin of the system." +
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

        private static void VerifyIfChangeToAdminMustBeAdmin(RegisteredUser user, string newState, ICloneableException<Exception> e)
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
