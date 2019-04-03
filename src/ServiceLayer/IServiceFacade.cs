using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    public interface IServiceFacade
    {
        /////Implements General Requirement 2.2
        /// <summary>
        /// Registers a user. Info should be valid, username should be unique.
        /// </summary>
        /// <PositiveTests>
        /// Valid string - non-null, more than 1 character long.
        /// 1. username is unique, is a Valid String, password is a Valid String
        /// 2. different username is unique, is a Valid String, same password
        /// </PositiveTests>
        /// <NegativeTests>
        /// 1. username and password is not Valid String (null, or empty)
        /// 2. username is Valid String, password is not.
        /// 3. username is not Vaid String, password is.
        /// 4. username is Valid String, password is Valid String, username is not unique
        /// </NegativeTests>
        /// <returns>True if registered successfully. False otherwise.</returns>
        bool Register(Guid userGuid, string username, string password);

        /////Implements General Requirement 2.3
        /// <summary>
        /// Logins a user using the username and password.
        /// </summary>
        /// <PositiveTests>
        /// 1. There is a registered user with these username and password, this user is not yet logged in.
        /// </PositiveTests>
        /// <NegativeTests>
        /// 1. There is a user with this username, but not with the same password
        /// 2. There is (at least) one user with this password, but with different username
        /// 3. There is a user with these username and password, but is already logged in.
        /// </NegativeTests>
        /// <returns>True if could login successfully. False otherwise.</returns>
        Guid Login(Guid userGuid, string username, string password);

        /////Implements General Requirement 3.1
        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <PositiveTests>
        /// 1. There is a logged in user with this Guid.
        /// </PositiveTests>
        /// <NegativeTests>
        /// 1. There is no logged-in user with this guid
        /// </NegativeTests>
        bool Logout(Guid userGuid);

        /////Implements General Requirement 3.2
        /// <summary>
        /// Opens a shop for the (logged-in) user.
        /// </summary>
        /// <PositiveTests>
        /// 1. Open a shop.
        /// 2. Open another shop.
        /// </PositiveTests>
        /// <NegativeTests>
        /// 1. Open a shop with a userGuid of no user.
        /// </NegativeTests>
        /// <returns>True if opened successfully. False otherwise.</returns>
        Guid OpenShop(Guid userGuid);

        /////Implements General Requirement 2.8. Not entirely, only purchase of a cart and not single items.
        /// <summary>
        /// Purchase the user's shopping cart of the specific shop.
        /// </summary>
        /// <constraints>
        /// 1. Must be a valid Guid of an existing user.
        /// 2. User must be logged in.
        /// 3. Must be a valid Guid of an existing shop.
        /// 4. Shop must be Active.
        /// 5. User must have at least one product bought from the shop.
        /// </constraints>
        /// <returns>True if purchased successfully. False otherwise.</returns>
        bool PurchaseCart(Guid userGuid, Guid shopGuid);

        /////Implements General Requirement 1.1
        /// <summary>
        /// Initializes the system.
        /// </summary>
        ///     <constraints>
        ///     1. All external services should be connectable.
        ///     2. Admin registration info should be provided and be valid if no Admin is registered.
        ///     </constraints>
        /// <returns>True if the system initialized successfully. False otherwise.</returns>
        bool Initialize(Guid userGuid, string username = null, string password = null);

        /////Implements General Requirement 6.2
        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <constraints>
        /// 1. User must be exist.
        /// 2. User must be logged in.
        /// 3. User must be in admin state.
        /// 4. UserToRemove must not be the only owner of an active shop.
        /// </constraints>
        /// <param name="username">True if the user was removed successfully. False otherwise.</param>
        /// <returns></returns>
        bool RemoveUser(Guid userGuid, Guid userToRemoveGuid);

        /////Implements General Requirement 7
        /// <summary>
        /// Checks if the system can connect to the payment system.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be admin.
        /// </constraints>
        /// <returns>True if can connect, false otherwise.</returns>
        bool ConnectToPaymentSystem(Guid userGuid);

        /////Implements General Requirement 8
        /// <summary>
        /// Checks if the system can connect to the supply system.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be admin.
        /// </constraints>
        /// <returns>True if can connect, false otherwise.</returns>
        bool ConnectToSupplySystem(Guid userGuid);

        /////Implements General Requirement 2.6
        /// <summary>
        /// Adds the product to the shopping cart.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 4. User must not have the item in the cart. (For edit - use EditProductInCart)
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in the shop.
        /// </constraints>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool AddProductToShoppingCart(Guid userGuid, Guid productGuid, Guid shopOfCartGuid);

        /////Implements General Requirement 2.7
        /// <summary>
        /// Gets all the products on the user's cart.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// </constraints>
        /// <returns>An enumerable collection of the products.</returns>
        IEnumerable<Guid> GetAllProductsInCart(Guid userGuid, Guid shopOfCartGuid);

        /////Implements General Requirement 2.7
        /// <summary>
        /// Removes the product from the user's cart.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in cart.
        /// </constraints>
        /// <returns>True if removed successfully. False otherwise.</returns>
        bool RemoveProductFromCart(Guid userGuid, Guid shopProductGuid, Guid shopOfCartGuid);

        /////Implements General Requirement 2.7
        /// <summary>
        /// Sets the amount of the product in the cart to the new amount.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in cart.
        /// 8. newAmount must be equal or greater than 1 (For Remove - user RemoveProductFromCart)
        /// </constraints>
        /// <returns>True if editted successfully. False otherwise.</returns>
        bool EditProductInCart(Guid userGuid, Guid shopOfCartGuid, Guid shopProductGuid, int newAmount);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Adds the product to the shop.
        /// </summary>
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
        /// <returns>True if added successfully. False otherwise.</returns>
        Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Removes the product from the shop.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner (or a manager with priviliges for this operation) of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. Product must exist in shop.
        /// </constraints>
        /// <returns>True if removed successfully. False otherwise.</returns>
        bool RemoveShopProduct(Guid userGuid, Guid shopProductGuid, Guid shopGuid);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Set the price and quantity fields of the product to the new parameters.
        /// </summary>
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
        /// <returns>True if editted successfully. False otherwise.</returns>
        bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);

        /////Implements General Requirement 2.5
        /// <summary>
        /// Returns a list of products in the shop with a name which contains the product name.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in buyer/guest state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. productName must not be null or string.Empty
        /// </constraints>
        /// <returns>A list of products.</returns>
        ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName);

        /////Implements General Requirement 4.3
        /// <summary>
        /// Adds the user as a shop owner.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. new shop manager must be an existing user.
        /// </constraints>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool AddShopOwner(Guid userGuid, Guid shopGuid, Guid newShopOwnerGuid);

        /////Implements General Requirement 4.4
        /// <summary>
        /// Removes the shop owner from owning the shop and cascade delete all owners appointed by him.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. ownerToRemove must not be the creator of the shop.
        /// 8. ownerToRemove must be an existing user.
        /// 9. ownerToRemove must have been appointed by the user with guid=userGuid
        /// </constraints>
        /// <returns>True if the operation was done successfully.</returns>
        bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid);

        /////Implements General Requirement 4.5
        /// <summary>
        /// Appoints the user as a shop manager of the shop.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 4. User must be an owner of the shop.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. newManagaerGuid must be an existing user.
        /// 8. newManagaerGuid must not be the creator of the shop, or one of the owners/managers.
        /// </constraints>
        /// <returns>True if the operation was done successfully.</returns>
        bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);

        /////Implements General Requirement 4.6
        /// <summary>
        /// Removes the user from managing the shop.
        /// </summary>
        /// <constraints>
        /// 1. Must be called by an existing user.
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// 5. Shop must exist.
        /// 6. Shop must be active.
        /// 7. newManagaerGuid must be an existing user.
        /// 8. newManagaerGuid must not be the creator of the shop, or one of the owners/managers.
        /// </constraints>
        /// <returns></returns>
        bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid);
    }
}
