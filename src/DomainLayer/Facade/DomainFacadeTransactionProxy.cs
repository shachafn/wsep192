using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccessLayer;
using ApplicationCore.Interfaces.DomainLayer;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.AddNewDiscountPolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4, field5);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddNewDiscountPolicy Constraints Failed.", e);
                    throw e;
                }
                catch(MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch(TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddNewDiscountPolicy Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        private void HandleMongoException(MongoCommandException mongoCommandException)
        {
            if (mongoCommandException.IsWriteConflictExcpetion())
            {
                ;//
            }
            //Need to find what is DbConnectionError and handle it
        }

        public Guid AddNewPurchasePolicy(UserIdentifier userGuid, Guid shopGuid, object policyType, object field1, object field2, object field3, object field4)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.AddNewPurchasePolicy(userGuid, shopGuid, policyType, field1, field2, field3, field4);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddNewPurchasePolicy Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddNewPurchasePolicy Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool AddProductToCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.AddProductToCart(userIdentifier, shopGuid, shopProductGuid, quantity);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid AddProductToShop(UserIdentifier userIdentifier, Guid shopGuid, string name, string category, double price, int quantity)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.AddProductToShop(userIdentifier, shopGuid, name, category, price, quantity);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool AddShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.AddShopManager(userIdentifier, shopGuid, newManagaerGuid, privileges);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddShopManager Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddShopManager Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool AddShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid newShopOwnerGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.AddShopOwner(userIdentifier, shopGuid, newShopOwnerGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddShopOwner Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddShopOwner Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public void cancelOwnerAssignment(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    _domainLayerFacade.cancelOwnerAssignment(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("cancelOwnerAssignment Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("cancelOwnerAssignment Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool CascadeRemoveShopOwner(UserIdentifier userIdentifier, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.CascadeRemoveShopOwner(userIdentifier, shopGuid, ownerToRemoveGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool ChangeUserState(UserIdentifier userIdentifier, string newState)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.ChangeUserState(userIdentifier, newState);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ChangeUserState Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ChangeUserState Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public void ClearSystem()
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    _domainLayerFacade.ClearSystem();
                    session.CommitTransaction();
                    isSuccess = true;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ClearSystem Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ClearSystem Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public void CloseShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    _domainLayerFacade.CloseShop(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("CloseShop Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("CloseShop Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public void CloseShopPermanently(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    _domainLayerFacade.CloseShopPermanently(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("CloseShopPermanently Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("CloseShopPermanently Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool ConnectToPaymentSystem(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.ConnectToPaymentSystem(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ConnectToPaymentSystem Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ConnectToPaymentSystem Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool ConnectToSupplySystem(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.ConnectToSupplySystem(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ConnectToSupplySystem Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ConnectToSupplySystem Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool EditProductInCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.EditProductInCart(userIdentifier, shopGuid, shopProductGuid, newAmount);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("EditProductInCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("AddProductToCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool EditProductInShop(UserIdentifier userIdentifier, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.EditProductInShop(userIdentifier, shopGuid, productGuid, newPrice, newQuantity);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("EditProductInShop Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("EditProductInShop Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public ICollection<ShopProduct> GetAllProductsInCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetAllProductsInCart(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetAllProductsInCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetAllProductsInCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public ICollection<Shop> GetAllShops(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetAllShops(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetAllShops Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetAllShops Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetAllUsersExceptMe(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetAllUsersExceptMe Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetAllUsersExceptMe Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetPurchaseHistory(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetPurchaseHistory Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetPurchaseHistory Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid GetShopGuid(string shopName)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetShopGuid(shopName);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetShopGuid Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetShopGuid Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public string GetShopName(Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetShopName(shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetShopName Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetShopName Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public IEnumerable<ShopProduct> GetShopProducts(UserIdentifier userId, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetShopProducts(userId, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetShopProducts Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetShopProducts Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetUserBag(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserBag Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserBag Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid GetUserGuid(string userName)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetUserGuid(userName);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserGuid Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserGuid Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public string GetUserName(Guid userGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetUserName(userGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserName Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserName Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public IEnumerable<Shop> GetUserShops(UserIdentifier userId)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetUserShops(userId);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserShops Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserShops Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid Initialize(UserIdentifier userIdentifier, string username, string password)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.Initialize(userIdentifier, username, password);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Initialize Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Initialize Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid Login(UserIdentifier userIdentifier, string username, string password)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.Login(userIdentifier, username, password);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Login Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Login Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool Logout(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.Logout(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Logout Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Logout Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid OpenShop(UserIdentifier userIdentifier)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.OpenShop(userIdentifier);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("OpenShop Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("OpenShop Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public Guid OpenShop(UserIdentifier userIdentifier, string shopName)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.OpenShop(userIdentifier, shopName);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("OpenShop Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("OpenShop Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool PurchaseCart(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.PurchaseCart(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("PurchaseCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("PurchaseCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }
        public double GetCartPrice(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.GetCartPrice(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetCartPrice Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetCartPrice Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }
        public Guid Register(UserIdentifier userIdentifier, string username, string password)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.Register(userIdentifier, username, password);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Register Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("Register Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool RemoveProductFromCart(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.RemoveProductFromCart(userIdentifier, shopGuid, shopProductGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveProductFromCart Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveProductFromCart Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool RemoveProductFromShop(UserIdentifier userIdentifier, Guid shopGuid, Guid shopProductGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.RemoveProductFromShop(userIdentifier, shopGuid, shopProductGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveProductFromShop Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveProductFromShop Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");

        }

        public bool RemoveShopManager(UserIdentifier userIdentifier, Guid shopGuid, Guid managerToRemoveGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.RemoveShopManager(userIdentifier, shopGuid, managerToRemoveGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveShopManager Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveShopManager Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool RemoveUser(UserIdentifier userIdentifier, Guid userToRemoveGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.RemoveUser(userIdentifier, userToRemoveGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveUser Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("RemoveUser Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public void ReopenShop(UserIdentifier userIdentifier, Guid shopGuid)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    _domainLayerFacade.ReopenShop(userIdentifier, shopGuid);
                    session.CommitTransaction();
                    isSuccess = true;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ReopenShop Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("ReopenShop Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(UserIdentifier userIdentifier, ICollection<string> toMatch, string searchType)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.SearchProduct(userIdentifier, toMatch, searchType);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("SearchProduct Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("SearchProduct Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }

        public bool IsUserAdmin(UserIdentifier id)
        {
            var isSuccess = false;
            var triesCount = 0;
            while (!isSuccess && triesCount < _maxRetriesCount)
            {
                var session = _unitOfWork.Context.StartSession();
                try
                {
                    session.StartTransaction();
                    var result = _domainLayerFacade.IsUserAdmin(id.Guid);
                    session.CommitTransaction();
                    isSuccess = true;
                    return result;
                }
                catch (BaseException e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserName Constraints Failed.", e);
                    throw e;
                }
                catch (MongoCommandException mongoExc)
                {
                    session.AbortTransaction();
                    HandleMongoException(mongoExc);
                }
                catch (TimeoutException e)
                {
                    session.AbortTransaction();
                    _logger.LogCritical("Got timeout from DB.", e);
                    throw new DatabaseConnectionTimeoutException();
                }
                catch (Exception e)
                {
                    session.AbortTransaction();
                    _logger.LogWarning("GetUserName Failed Due to unknown error.", e);
                    throw new GeneralServerError("An error has occured. Please try again.", e);
                }
                triesCount++;

            }
            throw new GeneralServerError("An error has occured. Please try again.");
        }
    }
}
