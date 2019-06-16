using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccessLayer;
using ApplicationCore.Interfaces.DomainLayer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DomainLayer.Facade
{
    public class DomainFacadeTransactionProxy : IDomainLayerFacade
    {
        DomainLayerFacade _domainLayerFacade;
        ILogger<DomainFacadeTransactionProxy> _logger;
        IUnitOfWork _unitOfWork;

        const int _maxRetriesCount = 3;

        public DomainFacadeTransactionProxy(DomainLayerFacade domainLayerFacade, ILogger<DomainFacadeTransactionProxy> logger, IUnitOfWork unitOfWork)
        {
            _domainLayerFacade = domainLayerFacade;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Guid AddNewDiscountPolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4, object field5)
        {
            /*
                         var isSuccess = false;
            var triesCount = 0;
            while(!isSuccess &&)
             */
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.AddNewDiscountPolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4, field5);
                session.CommitTransaction();
                return result;
            }
            catch(BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddNewDiscountPolicy Constraints Failed.", e);
                throw e;
            }
            catch(Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddNewDiscountPolicy Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid AddNewPurchasePolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.AddNewPurchasePolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddNewPurchasePolicy Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddNewPurchasePolicy Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.AddProductToCart(userIdentifier, shopGuid, shopProductGuid, quantity);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddProductToCart Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddProductToCart Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.AddProductToShop(userIdentifier, shopGuid, name, category, price, quantity);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddProductToShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddProductToShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.AddShopManager(userIdentifier, shopGuid, newManagaerGuid, privileges);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddShopManager Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddShopManager Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.AddShopOwner(userIdentifier, shopGuid, newShopOwnerGuid); 
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddShopOwner Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("AddShopOwner Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public void cancelOwnerAssignment(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                _domainLayerFacade.cancelOwnerAssignment(userIdentifier, shopGuid);
                session.CommitTransaction();
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("cancelOwnerAssignment Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("cancelOwnerAssignment Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.CascadeRemoveShopOwner(userIdentifier, shopGuid, ownerToRemoveGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("CascadeRemoveShopOwner Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("CascadeRemoveShopOwner Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.ChangeUserState(userIdentifier, newState);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ChangeUserState Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ChangeUserState Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public void ClearSystem()
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                _domainLayerFacade.ClearSystem();
                session.CommitTransaction();
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ClearSystem Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ClearSystem Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public void CloseShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                _domainLayerFacade.CloseShop(userIdentifier, shopGuid);
                session.CommitTransaction();
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("CloseShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("CloseShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                _domainLayerFacade.CloseShopPermanently(userIdentifier, shopGuid);
                session.CommitTransaction();
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("CloseShopPermanently Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("CloseShopPermanently Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.ConnectToPaymentSystem(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ConnectToPaymentSystem Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ConnectToPaymentSystem Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.ConnectToSupplySystem(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ConnectToSupplySystem Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ConnectToSupplySystem Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.EditProductInCart(userIdentifier, shopGuid, shopProductGuid, newAmount);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("EditProductInCart Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("EditProductInCart Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.EditProductInShop(userIdentifier, shopGuid, productGuid, newPrice, newQuantity);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("EditProductInShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("EditProductInShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public ICollection<ShopProduct> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetAllProductsInCart(userIdentifier, shopGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetAllProductsInCart Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetAllProductsInCart Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public ICollection<Shop> GetAllShops(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetAllShops(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetAllShops Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetAllShops Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetAllUsersExceptMe(userIdentifier); 
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetAllUsersExceptMe Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetAllUsersExceptMe Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetPurchaseHistory(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetPurchaseHistory Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetPurchaseHistory Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid GetShopGuid(string shopName)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetShopGuid(shopName);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetShopGuid Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetShopGuid Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public string GetShopName(Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetShopName(shopGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetShopName Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetShopName Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public IEnumerable<ShopProduct> GetShopProducts(UserIdentifier userId, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetShopProducts(userId, shopGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetShopProducts Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetShopProducts Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> getUserBag(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.getUserBag(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("getUserBag Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("getUserBag Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid GetUserGuid(string userName)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetUserGuid(userName);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserGuid Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserGuid Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public string GetUserName(Guid userGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetUserName(userGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserName Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserName Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public IEnumerable<Shop> GetUserShops(UserIdentifier userId)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetUserShops(userId);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserShops Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserShops Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.Initialize(userIdentifier, username, password);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Initialize Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Initialize Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid Login(UserIdentifier userIdentifier, string username, string password)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.Login(userIdentifier, username, password);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Login Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Login Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool Logout(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.Logout(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Logout Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Logout Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.OpenShop(userIdentifier);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("OpenShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("OpenShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public Guid OpenShop(UserIdentifier userIdentifier, string shopName)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.OpenShop(userIdentifier, shopName);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("OpenShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("OpenShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.PurchaseCart(userIdentifier, shopGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("PurchaseCart Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("PurchaseCart Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }
        public double GetCartPrice(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.GetCartPrice(userIdentifier, shopGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetCartPrice Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetCartPrice Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }
        public Guid Register(UserIdentifier userIdentifier, string username, string password)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.Register(userIdentifier, username, password);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Register Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("Register Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.RemoveProductFromCart(userIdentifier, shopGuid, shopProductGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveProductFromCart Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveProductFromCart Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.RemoveProductFromShop(userIdentifier, shopGuid, shopProductGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveProductFromShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveProductFromShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.RemoveShopManager(userIdentifier, shopGuid, managerToRemoveGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveShopManager Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveShopManager Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.RemoveUser(userIdentifier, userToRemoveGuid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveUser Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("RemoveUser Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public void ReopenShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                _domainLayerFacade.ReopenShop(userIdentifier, shopGuid);
                session.CommitTransaction();
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ReopenShop Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("ReopenShop Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.SearchProduct(userIdentifier, toMatch, searchType);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("SearchProduct Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("SearchProduct Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }

        public bool IsUserAdmin(UserIdentifier id)
        {
            var session = _unitOfWork.Context.StartSession();
            try
            {
                session.StartTransaction();
                var result = _domainLayerFacade.IsUserAdmin(id.Guid);
                session.CommitTransaction();
                return result;
            }
            catch (BaseException e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserName Failed.", e);
                throw e;
            }
            catch (Exception e)
            {
                session.AbortTransaction();
                _logger.LogWarning("GetUserName Failed Due to unknown error.", e);
                throw new GeneralServerError("An error has occured. Please try again.", e);
            }
        }
    }
}
