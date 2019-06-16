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
            var bag = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid);

            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.ShoppingBagDomain.CheckDiscountPolicy(bag, shopGuid);
            if (!_shopDomain.PurchaseCart(shop, bag))
                return false;
            //External payment pay, if not true ---- rollback
            return true;
        }
        public double GetCartPrice(Guid shopGuid)
        {
            var bag = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid);
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            _shopDomain.ShoppingBagDomain.CheckDiscountPolicyWithoutUpdate(bag, shopGuid);
            double totalPrice = _shopDomain.GetCartPrice(shop, bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shopGuid));
            _shopDomain.ShoppingBagDomain.ClearAllDiscounts(bag, shopGuid);
            return totalPrice;
        }

        public bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var bag = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid);
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            var actualProduct = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            _shopDomain.ShoppingBagDomain.AddProductToCart(bag, shopGuid, actualProduct, quantity);
            return true;
        }


        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var bag = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid);
            return _shopDomain.ShoppingBagDomain.EditProductInCart(bag, shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            var bag = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid);
            return _shopDomain.ShoppingBagDomain.RemoveProductFromCart(bag, shopGuid, shopProductGuid);
        }

        public ICollection<ShopProduct> GetAllProductsInCart(Guid shopGuid)
        {
            var cart = _unitOfWork.BagRepository.GetShoppingCartAndCreateIfNeeded(Guid, shopGuid);
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
            _unitOfWork.ShopRepository.Update(shop);
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

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag()
        {
            var bag = _unitOfWork.BagRepository.GetShoppingBagAndCreateIfNeeded(Guid);
            List<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> result = new List<Tuple<ShoppingCart, IEnumerable<ShopProduct>>>();
            if (bag != null && bag.ShoppingCarts != null)
            {
                foreach (var cart in bag.ShoppingCarts)
                {
                    List<ShopProduct> products = new List<ShopProduct>();
                    var shop = _unitOfWork.ShopRepository.FindByIdOrNull(cart.ShopGuid);
                    foreach (var item in cart.PurchasedProducts)
                    {
                        //ShopProduct currProduct = shop.ShopProducts.FirstOrDefault(prod => prod.Guid.Equals(item.Item1));
                        ShopProduct currProduct = item.Item1;
                        ShopProduct product = new ShopProduct();
                        product.Product = new Product(currProduct.Product.Name, currProduct.Product.Category);
                        product.Guid = currProduct.Guid;
                        product.Price = currProduct.Price;
                        product.Quantity = item.Item2;
                        products.Add(product);
                    }
                    result.Add(new Tuple<ShoppingCart, IEnumerable<ShopProduct>>(cart, products));
                }
            }
            return result;
        }

        #endregion
    }
}
