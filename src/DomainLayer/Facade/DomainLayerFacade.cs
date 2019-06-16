using ApplicationCore.Data;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Events;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccessLayer;
using ApplicationCore.Interfaces.DomainLayer;
using DomainLayer.Policies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DomainLayer.Facade
{
    public class DomainLayerFacade
    {
        IUserDomain _userDomain;
        DomainLayerFacadeVerifier _verifier;
        ILogger<DomainLayerFacade> _logger;
        IExternalServicesManager _externalServicesManager;
        IUnitOfWork _unitOfWork;

        public DomainLayerFacade(IUserDomain userDomain, DomainLayerFacadeVerifier verifier
            , ILogger<DomainLayerFacade> logger, IExternalServicesManager externalServicesManager
            , IUnitOfWork unitOfWork)
        {
            _userDomain = userDomain;
            _verifier = verifier;
            _externalServicesManager = externalServicesManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        private static bool _isSystemInitialized = false;

        public Guid Register(UserIdentifier userIdentifier, string username, string password)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            return _userDomain.Register(username, password, false);
        }

        public Guid Login(UserIdentifier userIdentifier, string username, string password)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            var result = _userDomain.Login(username, password);
            if (!result.Equals(Guid.Empty))
            {
                var newEvent = new UserLoggedInEvent(result);
                //newEvent.SetTargets(DomainData.ShopsCollection.Values, DomainData.RegisteredUsersCollection.Values);
                //newEvent.SetMessage(DomainData.ShopsCollection.Values, DomainData.RegisteredUsersCollection.Values);
                newEvent.SetMessages(_unitOfWork);
                UpdateCenter.RaiseEvent(newEvent);
                _logger.LogInformation($"{username} logged in successfuly.");
            }
            return result;
        }

        public bool Logout(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} logged out successfuly.");
            return _userDomain.LogoutUser(userIdentifier);
        }

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            var shopGuid = _userDomain.GetUserObject(userIdentifier).OpenShop();
            if (!shopGuid.Equals(Guid.Empty))
            {
                var newEvent = new OpenedShopEvent(userIdentifier.Guid, shopGuid);
                newEvent.SetMessages(_unitOfWork);
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} opened shop successfuly.");
                UpdateCenter.RaiseEvent(newEvent);
            }
            return shopGuid;
        }

        public Guid OpenShop(UserIdentifier userIdentifier, string shopName)
        {
            VerifySystemIsInitialized();
            if (shopName == null || shopName.Length == 0)
                return OpenShop(userIdentifier);
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopName);
            var shopGuid = _userDomain.GetUserObject(userIdentifier).OpenShop(shopName);
            if (!shopGuid.Equals(Guid.Empty))
            {
                var newEvent = new OpenedShopEvent(userIdentifier.Guid, shopGuid);
                newEvent.SetMessages(_unitOfWork);
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} opened shop {shopName} successfuly.");
                UpdateCenter.RaiseEvent(newEvent);
            }
            return shopGuid;
        }

        public void ReopenShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            _userDomain.GetUserObject(userIdentifier).ReopenShop(shopGuid);
            var newEvent = new ReopenedShopEvent(userIdentifier.Guid, shopGuid);
            newEvent.SetMessages(_unitOfWork);
            _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} reopened shop {GetShopName(shopGuid)} successfuly.");
            UpdateCenter.RaiseEvent(newEvent);
        }

        public void CloseShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            _userDomain.GetUserObject(userIdentifier).CloseShop(shopGuid);
            var newEvent = new ClosedShopEvent(userIdentifier.Guid, shopGuid);
            newEvent.SetMessages(_unitOfWork);
            _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} closed shop {GetShopName(shopGuid)} successfuly.");
            UpdateCenter.RaiseEvent(newEvent);
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            _userDomain.GetUserObject(userIdentifier).CloseShopPermanently(shopGuid);
            var newEvent = new ClosedShopPermanentlyEvent(userIdentifier.Guid, shopGuid);
            newEvent.SetMessages(_unitOfWork);
            _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} closed shop {GetShopName(shopGuid)} permanently successfuly.");
            UpdateCenter.RaiseEvent(newEvent);
        }

        public bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            // Need to actually pay for products
            // if success clear all carts
            //var newEvent = new PurchasedCartEvent(userIdentifier.Guid, shopGuid);
            //newEvent.SetMessages(DomainData.ShopsCollection.Values, DomainData.RegisteredUsersCollection.Values);
            //UpdateCenter.RaiseEvent(newEvent);
            var res =  _userDomain.GetUserObject(userIdentifier).PurchaseCart(shopGuid);
            if(res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} purchased cart from shop {GetShopName(shopGuid)} successfuly.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to purchase cart from shop {GetShopName(shopGuid)} successfuly.");
            return res;

        }

        public double GetCartPrice(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} got cart price from shop {GetShopName(shopGuid)} successfuly.");
            return _userDomain.GetUserObject(userIdentifier).GetCartPrice(shopGuid); ;
        }

        public Guid Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            username = username.ToLower();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, username, password);
            string msg;
            if (_isSystemInitialized)
                throw new SystemAlreadyInitializedException($"Cannot initialize the system again.");
            if (!_externalServicesManager.PaymentSystem.IsAvailable())
                ;// throw new ServiceUnReachableException($"Payment System Service is unreachable.");
            if (!_externalServicesManager.SupplySystem.IsAvailable())
                ;// throw new ServiceUnReachableException($"Supply System Service is unreachable.");

            var res = Guid.Empty;
            if (!_userDomain.IsAdminExists())
                _userDomain.Register(username, password, true);
            res = _userDomain.Login(username, password);
            _isSystemInitialized = res.Equals(Guid.Empty) ? false : true;
            return res;
        }

        public bool ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            bool res = _userDomain.GetAdminUser(userIdentifier).ConnectToPaymentSystem();
            if (res)
                _logger.LogCritical("The system connected to the payment system successfuly.");
            else
                _logger.LogCritical("The system failed to connect to the payment system.");
            return res;
        }

        public bool ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            bool res = _userDomain.GetAdminUser(userIdentifier).ConnectToSupplySystem();
            if (res)
                _logger.Log(LogLevel.Critical, "The system connected to the supply system successfuly.");
            else
                _logger.Log(LogLevel.Critical, "The system failed to connect to the supply system.");
            return res;
        }

        public Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, name, category, price, quantity);
            Guid res = _userDomain.GetUserObject(userIdentifier).AddProductToShop(shopGuid, name, category, price, quantity);
            if (!res.Equals(Guid.Empty))
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} added {name} " +
                    $"to shop {GetShopName(shopGuid)} successfuly. Category: {category}   Price: {price}   Quantity: {quantity}.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to add {name} to shop {GetShopName(shopGuid)} successfuly.");
            return res;
        }

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, productGuid, newPrice, newQuantity);
            ShopProduct product = GetShopProduct(shopGuid, productGuid);
            double oldPrice = product.Price;
            int oldQuantity = product.Quantity;
            _userDomain.GetUserObject(userIdentifier).EditProductInShop(shopGuid, productGuid, newPrice, newQuantity);
            _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} edited {product.Product.Name} in shop {GetShopName(shopGuid)} successfuly. " +
                $"Old Price: {oldPrice}   New Price: {product.Price}   Old Quantity: {oldQuantity}   New Quantity: {product.Quantity}.");
            return true;
        }

        public bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid);
            string productName = GetShopProductName(shopGuid, shopProductGuid);
            bool res = _userDomain.GetUserObject(userIdentifier).RemoveProductFromShop(shopGuid, shopProductGuid);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} removed" +
                    $" {productName} from shop {GetShopName(shopGuid)} successfuly.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to remove" +
                    $" {productName} from shop {GetShopName(shopGuid)}.");
            return res;
        }

        public bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid, quantity);
            bool res = _userDomain.GetUserObject(userIdentifier).AddProductToCart(shopGuid, shopProductGuid, quantity);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} added {quantity} " +
                    $" {GetShopProductName(shopGuid, shopProductGuid)} from shop {GetShopName(shopGuid)} to cart.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to add {quantity}" +
                    $" {GetShopProductName(shopGuid, shopProductGuid)} from shop {GetShopName(shopGuid)} to cart.");
            return res;
        }

        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, newManagaerGuid, privileges);
            bool res = _userDomain.GetUserObject(userIdentifier).AddShopManager(shopGuid, newManagaerGuid, privileges);
            string newManagerprivileges = privileges.Count == 0 ? "None" : string.Join('\n', privileges);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} added {GetUserName(newManagaerGuid)} " +
                    $" as a new manager of shop {GetShopName(shopGuid)} with privileges: {newManagerprivileges}.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to add {GetUserName(newManagaerGuid)}" +
                    $" as a new manager of shop {GetShopName(shopGuid)}.");
            return res;
        }

        public bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, newShopOwnerGuid);
            bool result = _userDomain.GetUserObject(userIdentifier).AddShopOwner(shopGuid, newShopOwnerGuid);
            if (result)
            {
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} added {GetUserName(newShopOwnerGuid)} " +
                    $" as a new owner of shop {GetShopName(shopGuid)}");
                var newEvent = new AddedOwnerEvent(newShopOwnerGuid, userIdentifier.Guid, shopGuid);
                newEvent.SetMessages(_unitOfWork);
                UpdateCenter.RaiseEvent(newEvent);
            }
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to add {GetUserName(newShopOwnerGuid)}" +
                    $" as a new owner of shop {GetShopName(shopGuid)}.");
            return result;
        }

        public bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, ownerToRemoveGuid);
            var result = _userDomain.GetUserObject(userIdentifier).CascadeRemoveShopOwner(shopGuid, ownerToRemoveGuid);
            if (result) //Maybe change CascadeRemoveShopOwner in IUser to return a collection of all removed owners.
            {
                _logger.Log(LogLevel.Information, $"{GetUserName(userIdentifier.Guid)} removed" +
                    $" {GetUserName(ownerToRemoveGuid)} as a shop owner from shop {GetShopName(shopGuid)} successfuly.");
                var newEvent = new RemovedOwnerEvent(ownerToRemoveGuid, userIdentifier.Guid, shopGuid);
                newEvent.SetMessages(_unitOfWork);
                UpdateCenter.RaiseEvent(newEvent);
            }
            else
            {
                _logger.Log(LogLevel.Information, $"{GetUserName(userIdentifier.Guid)} failed to remove" +
                    $" {GetUserName(ownerToRemoveGuid)} as a shop owner from shop {GetShopName(shopGuid)}.");
            }
            return result;
        }

        public bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid, newAmount);
            ShopProduct product = GetShopProduct(shopGuid, shopProductGuid);
            bool res = _userDomain.GetUserObject(userIdentifier).EditProductInCart(shopGuid, shopProductGuid, newAmount);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} edited {product.Product.Name} " +
                    $" in {GetShopName(shopGuid)} cart. new amount: {newAmount}");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to edit {product.Product.Name} " +
                    $" in {GetShopName(shopGuid)} cart.");
            return res;
        }

        public bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, shopProductGuid);
            bool res = _userDomain.GetUserObject(userIdentifier).RemoveProductFromCart(shopGuid, shopProductGuid);
            string productName = GetShopProductName(shopGuid, shopProductGuid);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} removed {productName} " +
                    $" from {GetShopName(shopGuid)} cart.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to remove {productName} " +
                    $" from {GetShopName(shopGuid)} cart.");
            return res;
        }

        public ICollection<ShopProduct> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            return _userDomain.GetUserObject(userIdentifier).GetAllProductsInCart(shopGuid);
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, userToRemoveGuid);
            string username = GetUserName(userToRemoveGuid);
            bool res = _userDomain.GetAdminUser(userIdentifier).RemoveUser(userToRemoveGuid);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} removed {username} " +
                    $" from the system.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to remove {username} " +
                    $" from the system.");
            return res;
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, toMatch, searchType);
            return _userDomain.GetUserObject(userIdentifier).SearchProduct(toMatch, searchType);
        }

        public bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid, managerToRemoveGuid);
            bool res = _userDomain.GetUserObject(userIdentifier).RemoveShopManager(shopGuid, managerToRemoveGuid);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} removed {GetUserName(managerToRemoveGuid)} " +
                    $" as a manager from shop {GetShopName(shopGuid)}.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to remove {GetUserName(managerToRemoveGuid)} " +
                    $" as a manager from shop {GetShopName(shopGuid)}.");
            return res;
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _logger.LogInformation($"Got purchase history for {GetUserName(userIdentifier.Guid)}.");
            return _userDomain.GetUserObject(userIdentifier).GetPurchaseHistory();
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier);
            return _userDomain.GetAllUsersExceptMe(userIdentifier);
        }

        public ICollection<Shop> GetAllShops(UserIdentifier userIdentifier)
        {
            VerifySystemIsInitialized();
            //  _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier); also guests can ask for shops
            return _unitOfWork.ShopRepository.FetchAll();
        }

        public IEnumerable<Shop> GetUserShops(UserIdentifier userId)
        {
            /*todo verofy constraints
             * VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);*/
            List<Shop> shops = _unitOfWork.ShopRepository.Query().Where(shop => shop.Creator.OwnerGuid.Equals(userId.Guid)).ToList();//created
            shops.AddRange(_unitOfWork.ShopRepository.Query().Where(shop => shop.Owners.Any(owner => owner.OwnerGuid.Equals(userId.Guid))));
            shops.AddRange(_unitOfWork.ShopRepository.Query().Where(shop => shop.Managers.Any(owner => owner.OwnerGuid.Equals(userId.Guid))));
            _logger.LogDebug($"Got all shops of user {GetUserName(userId.Guid)}.");
            return shops;
        }

        public IEnumerable<ShopProduct> GetShopProducts(UserIdentifier userIdentifier, Guid shopGuid)
        {
            /*todo verofy constraints
            * VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);*/
            Shop shop = GetShop(shopGuid);
            _logger.LogDebug($"{GetUserName(userIdentifier.Guid)} got all products of shop {shop.ShopName}.");
            return shop.ShopProducts;
        }

        public bool ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            VerifySystemIsInitialized();
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, newState);
            bool res = _userDomain.ChangeUserState(userIdentifier.Guid, newState);
            if (res)
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} changed to {newState}.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} faield to change to {newState}.");
            return res;
        }

        public void ClearSystem()
        {
            DomainData.ClearAll();
            _unitOfWork.ClearAll();
            _isSystemInitialized = false;
        }

        private Shop GetShop(Guid shopGuid) => _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);

        private void VerifySystemIsInitialized()
        {
            if (!_isSystemInitialized)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"System has not been initialized." +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                _logger.LogError(msg);
                throw new SystemNotInitializedException(msg);
            }
        }

        public Guid AddNewDiscountPolicy(UserIdentifier userIdentifier, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4, object field5)
        {
            VerifySystemIsInitialized();
            IUser user = _userDomain.GetUserObject(userIdentifier);
            IDiscountPolicy newPolicy = new UserDiscountPolicy();
            _verifier.AddNewDiscountPolicy(ref newPolicy, userIdentifier, shopGuid, policyType, field1, field2, field3, field4, field5);
            Guid discountPolicyGuid = user.AddNewDiscountPolicy(user.Guid, shopGuid, newPolicy);
            if (!discountPolicyGuid.Equals(Guid.Empty))
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} added new discount policy " +
                    $"of type {policyType.GetType()} to {GetShopName(shopGuid)}.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to add new discount policy " +
                    $"of type {policyType.GetType()} to {GetShopName(shopGuid)}.");

            return discountPolicyGuid;
        }

        public Guid AddNewPurchasePolicy(UserIdentifier userIdentifier, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4)
        {
            VerifySystemIsInitialized();
            IUser user = _userDomain.GetUserObject(userIdentifier);
            IPurchasePolicy newPolicy = new UserPurchasePolicy();
            _verifier.AddNewPurchasePolicy(ref newPolicy, userIdentifier, shopGuid, policyType, field1, field2, field3, field4);
            Guid purchasePoicyGuid = user.AddNewPurchasePolicy(user.Guid, shopGuid, newPolicy);
            if (!purchasePoicyGuid.Equals(Guid.Empty))
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} added new purchase policy " +
                    $"of type {policyType.GetType()} to {GetShopName(shopGuid)}.");
            else
                _logger.LogInformation($"{GetUserName(userIdentifier.Guid)} failed to add new purchase policy " +
                    $"of type {policyType.GetType()} to {GetShopName(shopGuid)}.");
            return purchasePoicyGuid;
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag(UserIdentifier userIdentifier, bool isGuest = false)
        {
            VerifySystemIsInitialized();
            _logger.LogInformation($"Got purchase history for {GetUserName(userIdentifier.Guid)}.");
            return _userDomain.GetUserObject(userIdentifier).GetUserBag();
        }

        public string GetUserName(Guid userGuid)
        {
            var user = _unitOfWork.BaseUserRepository.FindByIdOrNull(userGuid);
            return user == null ? "Guest - " + userGuid.ToString() : user.Username;
        }

        public Guid GetUserGuid(string userName) => _unitOfWork.BaseUserRepository.GetUserGuidByUsername(userName);

        public string GetShopName(Guid shopGuid) => _unitOfWork.ShopRepository.GetShopName(shopGuid);

        public ShopProduct GetShopProduct(Guid shopGuid, Guid productGuid)
        {
            return _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid).ShopProducts.FirstOrDefault(p => p.Guid.Equals(productGuid));
        }

        public string GetShopProductName(Guid shopGuid, Guid productGuid)
        {
            return GetShopProduct(shopGuid, productGuid).Product.Name;
        }

        public Guid GetShopGuid(string shopName)
        {
            return _unitOfWork.ShopRepository.GetShopGuidByName(shopName);
        }

        public void cancelOwnerAssignment(UserIdentifier userIdentifier, Guid shopGuid)
        {
            _verifier.VerifyMe(MethodBase.GetCurrentMethod(), userIdentifier, shopGuid);
            _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid).candidate = null;
        }

        public bool IsUserAdmin(Guid id)
        {
            var user =_unitOfWork.BaseUserRepository.FindByIdOrNull(id);
            if (user!=null)
            {
                return user.IsAdmin;
            }
            return false;
        }
    }
}
