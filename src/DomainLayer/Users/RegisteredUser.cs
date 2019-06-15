using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Domains;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Users
{
    public class RegisteredUser : IUser
    {
        protected IUnitOfWork _unitOfWork;
        protected ShopDomain _shopDomain;

        public Guid Guid { get => _baseUser.Guid; }
        public bool IsAdmin { get => _baseUser.IsAdmin; }
        private BaseUser _baseUser { get; set; }


        /// <summary>
        /// Default constructor, creates the user with a default Guest state.
        /// </summary>
        public RegisteredUser(BaseUser baseUser, IUnitOfWork unitOfWork, ShopDomain shopDomain)
        {
            _baseUser = baseUser;
            _unitOfWork = unitOfWork;
            _shopDomain = shopDomain;
        }

        #region Buyer

        public bool PurchaseCart(Guid shopGuid)
        {
            var cart = _unitOfWork.BagRepository.Query()
                .First(bag => bag.UserGuid.Equals(Guid))
                .ShoppingCarts
                .First(c => c.ShopGuid.Equals(shopGuid));

            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            //Can implement RollBack, purchase is given a Guid, shop.PurchaseCart returns a Guid,
            // if the user fails to pay later, we can delete the purchase and revert the shop quantities and cart content
            _shopDomain.ShoppingCartDomain.CheckDiscountPolicy(cart);
            if (!_shopDomain.PurchaseCart(shop, cart))
                return false;
            //External payment pay, if not true ---- rollback
            return true;
        }

        public bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var cart = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid).GetShoppingCartAndCreateIfNeeded(shopGuid);
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            var actualProduct = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            cart.AddProductToCart(actualProduct, quantity);
            return true;
        }


        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var cart = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid).GetShoppingCartAndCreateIfNeeded(shopGuid);
            return cart.EditProductInCart(shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            var cart = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid).GetShoppingCartAndCreateIfNeeded(shopGuid);
            return cart.RemoveProductFromCart(shopProductGuid);
        }

        public ICollection<ShopProduct> GetAllProductsInCart(Guid shopGuid)
        {
            var cart = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid).GetShoppingCartAndCreateIfNeeded(shopGuid);
            return cart.GetAllProductsInCart();
        }


        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            var searcher = new ProductsSearcher(searchType, _unitOfWork);
            return searcher.Search(toMatch);
        }

        #endregion

        #region Seller

        public Guid OpenShop()
        {
            var shop = new Shop(Guid);
            _unitOfWork.ShopRepository.Add(shop);
            return shop.Guid;
        }

        public Guid OpenShop(string shopName)
        {
            var shop = new Shop(Guid, shopName);
            _unitOfWork.ShopRepository.Add(shop);
            return shop.Guid;
        }

        public void ReopenShop(Guid shopGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            shop.Reopen();
        }

        public void CloseShop(Guid shopGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            shop.Close();
        }

        public void CloseShopPermanently(Guid shopGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            shop.ClosePermanently();
        }


        public Guid AddProductToShop(Guid shopGuid,
            string name, string category, double price, int quantity)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
           return _shopDomain.AddProductToShop(shop, Guid, new Product(name, category), price, quantity);
        }

        public void EditProductInShop(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.EditProductInShop(shop, Guid, productGuid, newPrice, newQuantity);
        }

        public bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.RemoveProductFromShop(shop, Guid, shopProductGuid);
            return true;
        }


        public bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.AddShopManager(shop, Guid, newManagaerGuid, privileges);
            return true;
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            return _shopDomain.CascadeRemoveShopOwner(shop, Guid, ownerToRemoveGuid);
        }


        public bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.RemoveShopManager(shop, Guid, managerToRemoveGuid);
            return true;
        }

        public bool AddShopOwner(Guid shopGuid, Guid newOwnerGuid)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.AddShopOwner(shop, Guid, newOwnerGuid);
            return true;
        }


        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            if (!(shop.IsOwner(userGuid) || (shop.IsManager(userGuid))))
            {
            throw new IllegalOperationException("Tried to add new purchase policy to a shop that doesn't belong to him");
            }
            return shop.AddNewPurchasePolicy(newPolicy);
        }

        public Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            if (!(shop.IsOwner(userGuid) || (shop.IsManager(userGuid))))
            {
            throw new IllegalOperationException("Tried to add new discount policy to a shop that doesn't belong to him");
            }
            return shop.AddNewDiscountPolicy(newPolicy);
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory()
        {
            return _unitOfWork.ShopRepository.Query().SelectMany(shop => shop.GetPurchaseHistory(Guid)).ToList();
        }

        #endregion
    }
}
