using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DAL;
using DomainLayer.Extension_Methods;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;

namespace DomainLayer.Users.States
{
    public class SellerUserState : AbstractUserState
    {
        public const string SellerUserStateString = "SellerUserState";

        public ICollection<Shop> ShopsOwned { get; set; }

        private IUnitOfWork _unitOfWork;
        public SellerUserState(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override ICollection<Guid> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Seller State");
        }

        public override Guid OpenShop(BaseUser baseUser)
        {
            var shop = new Shop(baseUser.Guid);
            ShopsOwned.Add(shop);
            _unitOfWork.ShopRepository.Create(shop);
            return shop.Guid;
        }

        public override Guid OpenShop(BaseUser baseUser, string shopName)
        {
            var shop = new Shop(baseUser.Guid, shopName);
            ShopsOwned.Add(shop);
            _unitOfWork.ShopRepository.Create(shop);
            return shop.Guid;
        }

        public override void ReopenShop(Guid shopGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindById(shopGuid);
            shop.Reopen();
        }

        public override void CloseShop(Guid shopGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindById(shopGuid);
            shop.Close();
        }

        public override void CloseShopPermanently(Guid shopGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindById(shopGuid);
            shop.ClosePermanently();
        }

        public override bool PurchaseCart(BaseUser baseUser, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke PurchaseBag in Seller State");
        }

        public override bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in Seller State");
        }

        public override bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in Seller State");
        }

        public override bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in Seller State");
        }

        public override Guid AddProductToShop(BaseUser baseUser, Guid shopGuid, 
            string name, string category, double price, int quantity)
        {
            return _unitOfWork.ShopRepository.FindById(shopGuid).AddProductToShop(baseUser.Guid, new Product(name, category), price, quantity);
        }

        public override void EditProductInShop(BaseUser baseUser,Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            _unitOfWork.ShopRepository.FindById(shopGuid).EditProductInShop(baseUser.Guid, productGuid, newPrice, newQuantity);
        }

        public override bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            _unitOfWork.ShopRepository.FindById(shopGuid).RemoveProductFromShop(baseUser.Guid, shopProductGuid);
            return true;
        }

        public override bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToCart in Seller State");
        }

        public override bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            _unitOfWork.ShopRepository.FindById(shopGuid).AddShopManager(baseUser.Guid, newManagaerGuid, priviliges);
            return true;
        }

        public override bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            return _unitOfWork.ShopRepository.FindById(shopGuid).CascadeRemoveShopOwner(baseUser.Guid, ownerToRemoveGuid, _unitOfWork);
        }

        public override bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Seller State");
        }

        public override bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Seller State");
        }

        public override ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Seller State");
        }

        public override bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid)
        {
            return _unitOfWork.ShopRepository.FindById(shopGuid).RemoveShopManager(baseUser.Guid, managerToRemoveGuid);
        }

        public override bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newOwnerGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindById(shopGuid);
            return shop.AddOwner(baseUser.Guid, newOwnerGuid);
        }
        public override ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            throw new BadStateException($"Tried to invoke SearchProduct in Seller State");
        }

        public override Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            var shop = _unitOfWork.ShopRepository.FindById(shopGuid);
            if (!shop.IsOwner(userGuid))
            {
                throw new IllegalOperationException("Tried to add new purchase policy to a shop that doesn't belong to him");
            }
            return shop.AddNewPurchasePolicy(newPolicy);
        }

        public override Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            var shop = _unitOfWork.ShopRepository.FindById(shopGuid);
            if (!shop.IsOwner(userGuid))
            {
                throw new IllegalOperationException("Tried to add new discount policy to a shop that doesn't belong to him");
            }
            return shop.AddNewDiscountPolicy(newPolicy);
        }
    }
}
