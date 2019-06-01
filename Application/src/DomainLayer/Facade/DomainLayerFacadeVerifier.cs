﻿using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ApplicationCore.Exceptions;
using ApplicationCore.Entities;
using ApplicationCore.Entitites;
using ApplicationCore.Data;
using static ApplicationCore.Entitites.Shop;
using DomainLayer.Users.States;
using DomainLayer.Extension_Methods;
using ApplicationCore.Entities.Users;
using DomainLayer.Policies;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Operators;

namespace DomainLayer.Facade
{
    public class DomainLayerFacadeVerifier
    {
        #region VerifyMe
        public void VerifyMe(MethodBase methodInfo, params object[] parameters)
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
                method.Invoke(this, parameters);
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
                if (is1 || is2 || is3 || is4 || is5 || is6 || is7 || is8)
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
        public void Register(UserIdentifier userIdentifier, string username, string password)
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
        public void Login(UserIdentifier userIdentifier, string username, string password)
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
        public void Logout(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new IllegalOperationException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public void OpenShop(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        public void OpenShop(UserIdentifier userIdentifier, string shopName)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyShopNameString(shopName, new IllegalArgumentException());
            VeriffyShopAlreadyExists(userIdentifier.Guid, shopName, new IllegalArgumentException());
        }

        public void ReopenShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            VerifyOwnerOfShop(userIdentifier.Guid, shopGuid, new OwnerNotFoundException());
            shop.VerifyShopIsActive();
        }

        public void CloseShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            VerifyOwnerOfShop(userIdentifier.Guid, shopGuid, new OwnerNotFoundException());
            shop.VerifyShopIsClosed();
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsPermanentlyClosed();
        }

        /////////// REDO CONSTRAINTS, CHANGED FROM CART TO BAG ////////////////
        public void PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
        }

        /// <constraints>
        /// 1. checked
        /// 2. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If an admin user exists - username and password must match it.
        /// 3. NOT IMPLEMENTED - SHOULD BE CHECKED HERE ----- If no admin user exists - username and password will be used to create one.
        /// 4. checked
        /// 5. if any external service is unavailable - return false
        /// </constraints>
        public void Initialize(UserIdentifier userIdentifier, string username, string password)
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
        public void RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
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
        public void ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        /// <constraints>
        /// 1. checked
        /// 2. checked
        /// 3. checked by design by state pattern
        /// </constraints>
        public void ConnectToSupplySystem(UserIdentifier userIdentifier)
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
        public void AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
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
        public void RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
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
        public void EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
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
        public void SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
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
        public void AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
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
        public void GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
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
        public void RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
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
        public void EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
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
        public void AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
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
        public void AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newOwnerGuid)
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
        public void CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            var shop = VerifyShopExists(shopGuid, new ShopNotFoundException());
            shop.VerifyShopIsActive();
            shop.VerifyOwnerOrCreator(userIdentifier.Guid, new NoPriviligesException());
            VerifyRegisteredUser(ownerToRemoveGuid, new UserNotFoundException());
            VerifyNotOwnerAppointer(userIdentifier.Guid, ownerToRemoveGuid, shopGuid, new IllegalArgumentException());
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
        public void RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
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
        public void ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
            VerifyString(newState, new IllegalArgumentException());
            VerifyStateString(newState, new IllegalArgumentException());
            VerifyIfChangeToAdminMustBeAdmin(user, newState, new IllegalOperationException());
        }

        public void AddNewPurchasePolicy(ref IPurchasePolicy policy, UserIdentifier userIdentifier, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null)
        {
            if (!(typeof(string) == policyType.GetType()))
                throw new IllegalArgumentException("Wrong policy type");
            string type = (string)policyType;
            switch (type)
            {
                case "User purchase policy":
                    VerifyUserShopPolicy(new IllegalArgumentException(), field1, field2, field3);
                    policy = new UserPurchasePolicy((string)field1, field2, (string)field3);
                    break;
                case "Product purchase policy":
                    VerifyProductPurchasePolicy(new IllegalArgumentException(), field1, field2, field3, field4);
                    policy = new ProductPurchasePolicy((Guid)field1, GetArithmeticOperator((string)field2), (int)field3, (string)field4);

                    break;
                case "Cart purchase policy":
                    if (!(typeof(int) == field2.GetType()))
                        throw new IllegalArgumentException("Invalid sum of cart");
                    policy = new CartPurchasePolicy((int)field2, GetArithmeticOperator((string)field1), (string)field3);
                    break;
                case "Compound purchase policy":
                    /*
                     field1 = p1 Guid
                     field2 = logical operator
                     field3 = p2 guid
                     field4 = description
                     */
                    VerifyCompositePurchasePolicy(new IllegalArgumentException(), field1, field2, field3, field4);
                    IPurchasePolicy p1 = VerifyPurchasePolicyExists(shopGuid, (Guid)field1, new PolicyNotFoundException());
                    IPurchasePolicy p2 = VerifyPurchasePolicyExists(shopGuid, (Guid)field3, new PolicyNotFoundException());
                    policy = new CompositePurchasePolicy(p1, GetLogicalOperator((string)field2), p2, (string)field4);

                    break;
                default:
                    throw new IllegalArgumentException("Invalid policy type");
            }
        }


        public void AddNewDiscountPolicy(ref IDiscountPolicy policy, UserIdentifier userIdentifier, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null, object field5 = null)
        {
            if (!(typeof(string) == policyType.GetType()))
                throw new IllegalArgumentException("Wrong policy type");
            string type = (string)policyType;
            switch (type)
            {
                case "Product discount policy":
                    //field1 = product guid
                    //field2 = operator
                    //field3 = expected amount
                    //field4 = discount percentage
                    //field5 = description
                    VerifyProductDiscountPolicy(new IllegalArgumentException(), field1, field2, field3, field4, field5);

                    policy = new ProductDiscountPolicy((Guid)field1, GetArithmeticOperator((string)field2), (int)field3, (int)field4, (string)field5);

                    break;
                case "Cart discount policy":
                    //field1 = Arithmetic operator
                    //field2 = sum of cart
                    //field3 = discount percentage
                    //field4 = description
                    VerifyCartDiscountPolicy(new IllegalArgumentException(), field1, field2, field3, field4);
                    policy = new CartDiscountPolicy(GetArithmeticOperator((string)field1), (double)field2, (int)field3, (string)field4);
                    break;
                case "User discount policy":
                    //field1 = field name
                    //field2 = expected value
                    //field3 = discount percentage
                    //field4 = description
                    VerifyUserShopPolicy(new IllegalArgumentException(), field1, field2, field3);
                    policy = new UserDiscountPolicy((string)field1, field2, (int)field3, (string)field4);
                    break;
                case "Compound discount policy":
                    /*
                     field1 = p1 Guid
                     field2 = logical operator
                     field3 = p2 guid
                     field4 = new discount percentage
                     field5 = description
                     */
                    VerifyCompositeDiscountPolicy(new IllegalArgumentException(), field1, field2, field3, field4, field5);
                    IDiscountPolicy p1 = VerifyDiscountPolicyExists(shopGuid, (Guid)field1, new PolicyNotFoundException());
                    IDiscountPolicy p2 = VerifyDiscountPolicyExists(shopGuid, (Guid)field3, new PolicyNotFoundException());
                    policy = new CompositeDiscountPolicy(p1, GetLogicalOperator((string)field2), p2, (int)field4, (string)field5);

                    break;

                default:
                    throw new IllegalArgumentException("Invalid policy type");
            }
        }


        public void GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        public void GetAllShops(UserIdentifier userIdentifier)
        {
            var user = VerifyLoggedInUser(userIdentifier.Guid, new UserNotFoundException());
        }

        #endregion
        #region Operators
        private IArithmeticOperator GetArithmeticOperator(string input)
        {
            switch (input)
            {
                case ">":
                    return new BiggerThan();
                case "<":
                    return new SmallerThan();
                default:
                    return null;
            }
        }

        private ILogicOperator GetLogicalOperator(string input)
        {
            switch (input)
            {
                case "&":
                    return new And();
                case "->":
                    return new Implies();
                case "|":
                    return new Or();
                case "^":
                    return new Xor();
                default:
                    return null;
            }
        }
        #endregion
        #region Private Verifiers
        private IUser VerifyLoggedInUser(Guid userGuid, ICloneableException<Exception> e)
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

        private Shop VerifyShopExists(Guid shopGuid, ICloneableException<Exception> e)
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

        private IPurchasePolicy VerifyPurchasePolicyExists(Guid shopGuid, Guid purchasePolicyGuid, ICloneableException<Exception> e)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            foreach (IPurchasePolicy policy in shop.PurchasePolicies)
            {
                if (policy.Guid.Equals(purchasePolicyGuid))
                    return policy;
            }
            throw new PolicyNotFoundException();
        }

        private void VerifyGuestUser(UserIdentifier userIdentifier, ICloneableException<Exception> e)
        {
            if (!userIdentifier.IsGuest)
            {
                StackTrace stackTrace = new StackTrace();
                throw e.Clone($"User with Guid - {userIdentifier} is not a guest." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private void VerifyRegisteredUser(Guid userGuid, ICloneableException<Exception> e)
        {
            if (!DomainData.RegisteredUsersCollection.ContainsKey(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = string.Format(Resources.EntityNotFound, "registered user", userGuid) +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyString(string str, ICloneableException<Exception> e)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"String is null, empty or whitespace." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyShopNameString(string str, ICloneableException<Exception> e)
        {
            if (str == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"String is null." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VeriffyShopAlreadyExists(Guid userGuid, string shopName, ICloneableException<Exception> e)
        {
            var constraint = DomainData.ShopsCollection.Where(shop => shop.Creator.OwnerGuid.Equals(userGuid)
                || shop.Owners.Select(owner => owner.OwnerGuid).Contains(userGuid)).Select(shop => shop.ShopName).Contains(shopName);
            if (constraint)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {userGuid} is already an owner of shop {shopName}." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyDoubleGreaterThan0(double toCheck, ICloneableException<Exception> e)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyIntEqualOrGreaterThan0(int toCheck, ICloneableException<Exception> e)
        {
            if (toCheck < 0)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyIntGreaterThan0(int toCheck, ICloneableException<Exception> e)
        {
            if (toCheck <= 0)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"{toCheck} is not equal or greater than 0" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyOwnerOfShop(Guid userGuid, Guid shopGuid, ICloneableException<Exception> e)
        {
            var shop = DomainData.ShopsCollection.FirstOrDefault(s => s.Guid.Equals(shopGuid));
            var constraint = shop.Owners.Select(owner => owner.OwnerGuid).Contains(userGuid)
                || shop.Creator.OwnerGuid.Equals(userGuid);
            if (!constraint)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {userGuid} is the not an owner of the shop {shop.ShopName}." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyNotOwnerAppointer(Guid userGuid, Guid ownerToRemoveGuid, Guid shopGuid, ICloneableException<Exception> e)
        {
            var shop = DomainData.ShopsCollection.FirstOrDefault(s => s.Guid.Equals(shopGuid));
            var constraint = !(shop.Creator.OwnerGuid.Equals(userGuid)) && shop.Owners.FirstOrDefault(owner => owner.OwnerGuid.Equals(userGuid)).AppointerGuid.Equals(ownerToRemoveGuid);
            if (constraint)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {userGuid} cannot remove user with Guid - {ownerToRemoveGuid} from {shop.ShopName} because this user is it's appointer ." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyNotOnlyOwnerOfAnActiveShop(Guid userGuid, ICloneableException<Exception> e)
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

        private ShoppingCart GetCartExistsAndCreateIfNeeded(UserIdentifier userIdentifier, Guid shopGuid)
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

        private void VerifyStateString(string newState, ICloneableException<Exception> e)
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

        private void VerifySearchString(string searchType, ICloneableException<Exception> e)
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

        private void VerifySearchInput(ICollection<string> toMatch, ICloneableException<Exception> e)
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

        private void VerifyNotOnlyAdmin(Guid userToRemoveGuid, ICloneableException<Exception> e)
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

        private void VerifyLoginCredentials(string username, string password, ICloneableException<Exception> e)
        {
            if (!DomainData.RegisteredUsersCollection.Any(u => u.Username.ToLower().Equals(username.ToLower()) && u.CheckPass(password)))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Login Credentials does not match any registered user." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyIfChangeToAdminMustBeAdmin(IUser user, string newState, ICloneableException<Exception> e)
        {
            if (newState.Equals(AdminUserState.AdminUserStateString) && !user.IsAdmin)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"Can't change state to admin state with the user not being an admin." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        private void VerifyCart(ShoppingCart cart, ICloneableException<Exception> e)
        {
            Shop shop = DomainData.ShopsCollection[cart.ShopGuid];
            BaseUser user = DomainData.RegisteredUsersCollection[cart.UserGuid];
            foreach (Tuple<Guid, int> record in cart.PurchasedProducts)
            {
                foreach (ProductPurchasePolicy policy in shop.PurchasePolicies)
                {
                    bool result = policy.CheckPolicy(cart, record.Item1, record.Item2, user);
                    if (result == false)
                        throw e.Clone("Broken constraint: " + policy.ToString());
                }
            }
        }

        public void VerifyShopProductDoesNotExist(ShoppingCart cart, Guid shopProductGuid, ICloneableException<Exception> e)
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

        public void VerifyShopProductExistsInCart(ShoppingCart cart, Guid shopProductGuid, ICloneableException<Exception> e)
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

        private void VerifyUserShopPolicy(ICloneableException<Exception> e, object field1, object field2, object field3 = null)
        {
            //Assumption: property is known and legal.
            //Assumption : no operator is needed.
            BaseUser b = new BaseUser("userDemo", "000000", false);
            FieldInfo[] baseUserFields = b.GetType().GetFields();
            foreach (FieldInfo baseUserField in baseUserFields)
            {
                if (baseUserField.Name == (string)field1 && baseUserField.FieldType != field3.GetType())
                    e.Clone("Mismatch between property type");
            }

        }

        private void VerifyProductPurchasePolicy(ICloneableException<Exception> e, object field1, object field2, object field3, object field4)
        {
            VerifyGuid(field1);
            VerifyArithmeticOperator(field2);
            VerifyInt(field3);
            VerifyString((string)field4, e);
        }

        private void VerifyCompositePurchasePolicy(ICloneableException<Exception> e, object field1, object field2, object field3, object field4)
        {
            /*
            field1 = p1 Guid
            field2 = logical operator
            field3 = p2 guid
            field4 = description
            */
            VerifyGuid(field1);
            VerifyLogicalOperator(field2);
            VerifyGuid(field3);
            VerifyString((string)field4, e);
        }

        private void VerifyCompositeDiscountPolicy(ICloneableException<Exception> e, object field1, object field2, object field3, object field4, object field5)
        {
            /*
             field1 = p1 Guid
             field2 = logical operator
             field3 = p2 guid
             field4 = new discount percentage
             field5 = description
                   */
            VerifyGuid(field1);
            VerifyLogicalOperator(field2);
            VerifyGuid(field3);
            VerifyInt(field4);
            VerifyString((string)field5, e);

        }
        private IDiscountPolicy VerifyDiscountPolicyExists(Guid shopGuid, Guid discountPolicyGuid, PolicyNotFoundException policyNotFoundException)
        {
            var shop = DomainData.ShopsCollection[shopGuid];
            foreach (IDiscountPolicy policy in shop.DiscountPolicies)
            {
                if (policy.Guid.Equals(discountPolicyGuid))
                    return policy;
            }
            throw new PolicyNotFoundException();
        }

        private void VerifyCartDiscountPolicy(IllegalArgumentException e, object field1, object field2, object field3, object field4)
        {
            //field1 = Arithmetic operator
            //field2 = sum of cart
            //field3 = discount percentage
            //field4 = description
            VerifyArithmeticOperator(field1);
            VerifyDoubleOrInt(field2);
            VerifyInt(field3);
            VerifyString((string)field4, e);

        }

        private void VerifyArithmeticOperator(object toCheck)
        {
            if (!(typeof(string) == toCheck.GetType()))
                throw new IllegalArgumentException();
            if (GetArithmeticOperator((string)toCheck) == null)
                throw new IllegalArgumentException();
        }

        private void VerifyDoubleOrInt(object toCheck)
        {
            if (!(typeof(double) == toCheck.GetType() || typeof(int) == toCheck.GetType()))
                throw new IllegalArgumentException();
        }

        private void VerifyProductDiscountPolicy(IllegalArgumentException e, object field1, object field2, object field3, object field4, object field5)
        {
            VerifyGuid(field1);
            VerifyArithmeticOperator(field2);
            VerifyInt(field3);
            VerifyInt(field4);
            VerifyString((string)field5, e);
        }

        private void VerifyGuid(object toCheck)
        {
            if (!(typeof(Guid) == toCheck.GetType()))
                throw new IllegalArgumentException();
        }

        private void VerifyLogicalOperator(object toCheck)
        {
            if (!(typeof(string) == toCheck.GetType()))
                throw new IllegalArgumentException();
            if (GetLogicalOperator((string)toCheck) == null)
                throw new IllegalArgumentException();
        }

        private void VerifyInt(object toCheck)
        {
            if (!(typeof(int) == toCheck.GetType()))
                throw new IllegalArgumentException();
        }

        #endregion
    }
}