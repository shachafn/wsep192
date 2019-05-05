using System;
using System.Collections.Generic;

namespace ATBridge
{
    public interface IBridge
    {
        /////Implements General Requirement 2.2
        /// <summary>
        /// Registers a user. Info should be valid, username should be unique.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. userGuid must be GuestGuid.
        /// 2. username and password must not be string.IsNullOrWhitespace
        /// 3. if username is taken - return false;
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="IllegalOperationException">When the userGuid is not a GuestGuid.</exception>
        /// <exception cref="IllegalArgumentException">When the username/password are null, empty or whitespace</exception>
        /// <returns>The Guid of the created user, Guid.Empty if the username is taken.</returns>
        Guid Register(Guid userGuid, string username, string password);

        /////Implements General Requirement 2.3
        /// <summary>
        /// Logins a user using the username and password.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. userGuid must be GuestGuid.
        /// 2. username and password must not be string.IsNullOrWhitespace
        /// 3. if username and password doesnt match nay user - return false
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="IllegalOperationException">When the userGuid is not a GuestGuid.</exception>
        /// <exception cref="IllegalArgumentException">When the username/password are null, empty or whitespace</exception>
        /// <exception cref="CredentialsMismatchException">When the username and password does not match any registered user's credentials</exception>
        /// <returns>true if logged in successfully. False otherwise.</returns>
        bool Login(Guid userGuid, string username, string password);

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. User must exist
        /// 2. User must be logged in.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="IllegalOperationException">When userGuid does not match any logged-in user's guid.</exception>
        /// <returns>True.</returns>
        bool Logout(Guid userGuid);

        /////Implements General Requirement 3.2
        /// <summary>
        /// Opens a shop for the (logged-in) user.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. User must exist
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in seller state</exception>
        /// <returns>Guid of the created shop.</returns>
        Guid OpenShop(Guid userGuid);

        /////Implements General Requirement 2.8. Not entirely, only purchase of the entier bag.
        /////////////// REDO CONSTRAINTS, CHANGED FROM CART TO BAG ////////////////////////
        bool PurchaseBag(Guid userGuid);


        /////Implements General Requirement 2.8. Not entirely, only purchase of the entier bag.
        /////////////// REDO CONSTRAINTS, CHANGED FROM CART TO BAG ////////////////////////
        bool PurchaseCart(Guid userGuid , Guid shopGuid);


        /////Implements General Requirement 1.1
        /// <summary>
        /// Initializes the system. If there is already an admin user - username and password must match it.
        /// If there is no admin user yet, one is created using these username and password.
        /// This function also loggs the admin user into the system, and changes its state to admin state.
        /// </summary>
        /// <constraints>
        /// 1. userGuid must be GuestGuid (cant login if system not initialized)
        /// 2. If an admin user exists - username and password must match it.
        /// 3. If no admin user exists - username and password will be used to create one.
        /// 4. username and password must not be string.NullOrWhitespace
        /// 5. if any external service is unavailable - an exception is thrown
        /// </constraints>
        /// <exception cref="SystemAlreadyInitializedException">When system has already been initialized.</exception>
        /// <exception cref="BrokenConstraintException">When the userGuid is not a GuestGuid.</exception>
        /// <exception cref="IllegalArgumentException">When the username/password are null, empty or whitespace</exception>
        /// <exception cref="CredentialsMismatchException">When the username/password does not match the admin user's credentials</exception>
        /// <exception cref="ServiceUnReachableException">When an external service is unreachable.</exception>
        /// <returns>Guid of the admin user.</returns>
        Guid Initialize(Guid userGuid, string username, string password);

        /////Implements General Requirement 6.2
        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. User must exist.
        /// 2. User must be logged in.
        /// 3. User must be in admin state.
        /// 4. UserToRemove must not be the only owner of an active shop.
        /// 5. UserToRemove must not be the only admin of the system.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in AdminUserState</exception>
        /// <exception cref="BrokenConstraintException">When the user is the only owner of an active shop</exception>
        /// <exception cref="BrokenConstraintException">When the user is the only admin of the system</exception>
        /// <returns>True.</returns>
        bool RemoveUser(Guid userGuid, Guid userToRemoveGuid);

        /////Implements General Requirement 7
        /// <summary>
        /// Checks if the system can connect to the payment system.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be admin.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in AdminUserState</exception>
        /// <returns>True if can connect, false otherwise.</returns>
        bool ConnectToPaymentSystem(Guid userGuid);

        /////Implements General Requirement 8
        /// <summary>
        /// Checks if the system can connect to the supply system.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be admin.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in AdminUserState</exception>
        /// <returns>True if can connect, false otherwise.</returns>
        bool ConnectToSupplySystem(Guid userGuid);

        /////Implements General Requirement 2.6
        /// <summary>
        /// Adds the product to the shopping cart.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 4. User must not have the item in the cart. (For edit - use EditProductInCart)
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in the shop.
        /// 8. quantity must be greater than 0
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in BuyerUserState</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="IllegalArgumentException">When the quantity is not greter than 0</exception>
        /// <exception cref="ProductNotFoundException">When the productGuid does not match any product in the shop.</exception>
        /// <exception cref="BrokenConstraintException">When the product already exists in the user's cart.</exception>
        /// <returns>True.</returns>
        bool AddProductToCart(Guid userGuid, Guid productGuid, Guid shopGuid, int quantity);

        /////Implements General Requirement 2.7
        /// <summary>
        /// Gets all the products on the user's cart.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in BuyerUserState</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <returns>An enumerable collection of the products.</returns>
        ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid);

        /////Implements General Requirement 2.7
        /// <summary>
        /// Removes the product from the user's cart.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in cart.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in BuyerUserState</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="BrokenConstraintException">When shopProductGuid does not match in product in the cart.</exception>
        /// <returns>True.</returns>
        bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid);

        /////Implements General Requirement 2.7
        /// <summary>
        /// Sets the amount of the product in the cart to the new amount.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in cart.
        /// 8. newAmount must be equal or greater than 1 (For Remove - user RemoveProductFromCart)
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in BuyerUserState</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="BrokenConstraintException">When shopProductGuid does not match in product in the cart.</exception>
        /// <returns>True.</returns>
        bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Adds the product to the shop.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be creator, or an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. name must not be string.IsNullOrWhitespace
        /// 8. category must not be string.IsNullOrWhitespace
        /// 9. price must be grater than 0
        /// 10. quantity must be equal or greater than 0 (May not have any to sell at the moment).
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not a creator,owner or manager with priviliges.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="IllegalArgumentException">When the name/category is null,empty or whitespace</exception>
        /// <exception cref="IllegalArgumentException">When price is not greater than 0.</exception>
        /// <exception cref="IllegalArgumentException">When the quantity is not equal or greater than 0.</exception>
        /// <returns>True if added successfully. False otherwise.</returns>
        Guid AddProductToShop(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Removes the product from the shop.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be creator, an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in shop.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not a creator,owner or manager with priviliges.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="ProductNotFoundException">When shopProductGuid does not match any product in the shop.</exception>
        /// <returns>True if removed successfully. False otherwise.</returns>
        bool RemoveProductFromShop(Guid userGuid, Guid shopGuid, Guid shopProductGuid);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Set the price and quantity fields of the product to the new parameters.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be creator, an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in shop.
        /// 8. newPrice must be greater than 0.
        /// 9. newQuantity must be equal or greater than 0 (May not have any to sell at the moment).
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not a creator,owner or manager with priviliges.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="ProductNotFoundException">When shopProductGuid does not match any product in the shop.</exception>
        /// <exception cref="IllegalArgumentException">When newPrice is not greater than 0.</exception>
        /// <exception cref="IllegalArgumentException">When newQuantity is not equal or greater than 0</exception>
        /// <returns>True if editted successfully. False otherwise.</returns>
        bool EditProductInShop(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);

        /////Implements General Requirement 2.5
        /// <summary>
        /// Returns a list of products in all shops who match the search criterea:
        /// 1. By name - return all products in all shops who contain the name.
        /// 2. By Category - return all products in all shops who match this category.
        /// 3. By keywords - return all products in all shops who contains these keyords.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. searchType must be "Name" or "Category" or "Keywords".
        /// 2. toMatch must not be empty.
        /// 3. toMatch strings must not be string.IsNullOrWhitespace
        /// 4. State must be Guest/Buyer
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="BadStateException">When the user is not in BuyerUserState/GuestUserState</exception>
        /// <exception cref="IllegalArgumentException">When searchType is not "Name" or "Category" or "Keywords".</exception>
        /// <exception cref="IllegalArgumentException">When toMatch contains illegal strings.</exception>
        /// <exception cref="IllegalArgumentException">When toMatch is empty.</exception>
        /// <returns>A list of products.</returns>
        ICollection<Guid> SearchProduct(Guid userGuid, ICollection<string> toMatch, string searchType);

        /////Implements General Requirement 4.3
        /// <summary>
        /// Adds the user as a shop owner.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. new shop manager must be an existing user.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not an owner of the shop.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="UserNotFoundException">When the newShopOwnerGuid does not match any registered user's guid</exception>
        /// <exception cref="BrokenConstraintException">When the newShopOwner is already an owner</exception>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid);

        /////Implements General Requirement 4.4
        /// <summary>
        /// Removes the shop owner from owning the shop and cascade delete all owners appointed by him.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. ownerToRemove must be an owner of the shop.
        /// 8. ownerToRemove must have been appointed by the user with guid=userGuid
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not an owner of the shop.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="UserNotFoundException">When the newShopOwnerGuid does not match any registered user's guid</exception>
        /// <exception cref="BrokenConstraintException">When the newShopOwner is already an owner</exception>
        /// <returns>True if the operation was done successfully.</returns>
        bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid);

        /////Implements General Requirement 4.5
        /// <summary>
        /// Appoints the user as a shop manager of the shop.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. newManagaerGuid must be an existing user.
        /// 8. newManagaerGuid must not be the creator of the shop, or one of the owners/managers.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not an owner of the shop.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="UserNotFoundException">When the newManagaerGuid does not match any registered user's guid</exception>
        /// <exception cref="BrokenConstraintException">When the newShopOwner is already a creator/owner/manager</exception>
        /// <returns>True if the operation was done successfully.</returns>
        bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);

        /////Implements General Requirement 4.6
        /// <summary>
        /// Removes the user from managing the shop.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be owner
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. newManagaerGuid must be an existing user.
        /// 8. newManagaerGuid must not be the creator of the shop, or one of the owners/managers.
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="BadStateException">When the user is not in SellerUserState</exception>
        /// <exception cref="NoPriviligesException">When the user is not an owner of the shop.</exception>
        /// <exception cref="ShopNotFoundException">When shopGuid does not match any existing shop guid.</exception>
        /// <exception cref="ShopStateException">When the shop is not active.</exception>
        /// <exception cref="UserNotFoundException">When the newManagaerGuid does not match any registered user's guid</exception>
        /// <exception cref="BrokenConstraintException">When the newShopOwner is already a creator/owner/manager</exception>
        /// <returns></returns>
        bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid);

        /// <summary>
        /// Changes the user's state to the new state.
        /// </summary>
        /// <constraints>
        /// 0. System must be Initialized.
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. newState must not be string.IsNullOrWhitespace
        /// 4. newState must be a valid state (see implementation)
        /// </constraints>
        /// <exception cref="SystemNotInitializedException">When system has not yet been initialized.</exception>
        /// <exception cref="UserNotFoundException">When userGuid does not match any logged-in user's guid.</exception>
        /// <exception cref="IllegalArgumentException">When the newState is not one of "AdminUserState","BuyerUserState","SellerUserState"</exception>
        /// <exception cref="IllegalOperationException">When user tries to change state to admin, but is not an admin"</exception>
        /// <returns></returns>
        bool ChangeUserState(Guid userGuid, string newState);


        Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null);
        Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3 = null, object field4 = null);

        /// <summary>
        /// Clears all data and requires the system to be Initialized to use it.
        /// </summary>
        void ClearSystem();
    }
}
