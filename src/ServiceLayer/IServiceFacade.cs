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
        /// <constraints>
        /// 1. userGuid must be GuestGuid.
        /// 2. username and password must not be string.IsNullOrWhitespace
        /// 3. if username is taken - return false;
        /// </constraints>
        /// <returns>True if registered successfully. False otherwise.</returns>
        bool Register(Guid userGuid, string username, string password);

        /////Implements General Requirement 2.3
        /// <summary>
        /// Logins a user using the username and password.
        /// </summary>
        /// <constraints>
        /// 1. userGuid must be GuestGuid.
        /// 2. username and password must not be string.IsNullOrWhitespace
        /// 3. if username and password doesnt match nay user - return false
        /// </constraints>
        /// <returns>True if could login successfully. False otherwise.</returns>
        Guid Login(Guid userGuid, string username, string password);

        /////Implements General Requirement 3.1
        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <constraints>
        /// 1. User must exist
        /// 2. User must be logged in.
        /// </constraints>
        bool Logout(Guid userGuid);

        /////Implements General Requirement 3.2
        /// <summary>
        /// Opens a shop for the (logged-in) user.
        /// </summary>
        /// <constraints>
        /// 1. User must exist
        /// 2. User must be logged in.
        /// 3. User must be in seller state.
        /// </constraints>
        /// <returns>True if opened successfully. False otherwise.</returns>
        Guid OpenShop(Guid userGuid);

        /////Implements General Requirement 2.8. Not entirely, only purchase of a cart and not single items.
        /// <summary>
        /// Purchase the user's shopping cart of the specific shop.
        /// </summary>
        /// <constraints>
        /// 1. User must exist
        /// 2. User must be logged in.
        /// 3. User must be in buyer state.
        /// 4. Shop must exist
        /// 5. Shop must be active. 
        /// 6. User must have at least one item in cart.
        /// </constraints>
        /// <returns>True if purchased successfully. False otherwise.</returns>
        bool PurchaseCart(Guid userGuid, Guid shopGuid);

        /////Implements General Requirement 1.1
        /// <summary>
        /// Initializes the system. If there is already an admin user - username and password must match it.
        /// If there is no admin user yet, one is created using these username and password.
        /// </summary>
        /// <constraints>
        /// 1. userGuid must be GuestGuid (cant login if system not initialized)
        /// 2. If an admin user exists - username and password must match it.
        /// 3. If no admin user exists - username and password will be used to create one.
        /// 4. username and password must not be string.NullOrWhitespace
        /// 5. if any external service is unavailable - return false
        /// </constraints>
        /// <returns>True if the system initialized successfully. False otherwise.</returns>
        bool Initialize(Guid userGuid, string username, string password);

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
        /// 8. quantity must be greater than 0
        /// </constraints>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool AddProductToShoppingCart(Guid userGuid, Guid productGuid, Guid shopGuid, int quantity);

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
        ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid);

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
        bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid);

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
        bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount);

        /////Implements General Requirement 4.1
        /// <summary>
        /// Adds the product to the shop.
        /// </summary>
        /// <constraints>
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
        /// 4. User must be creator, an owner (or a manager with priviliges for this operation) of the shop.
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
        /// 4. User must be creator, an owner (or a manager with priviliges for this operation) of the shop.
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
