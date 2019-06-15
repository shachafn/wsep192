using ApplicationCore.Data;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer;
using DomainLayer.Domains;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Entities.Users
{
    public class GuestUser : IUser
    {
        protected IUnitOfWork _unitOfWork;
        protected ShopDomain _shopDomain; 

        public Guid Guid { get; private set; }
        public bool IsAdmin => false;
        
        public GuestUser(Guid guid, IUnitOfWork unitOfWork, ShopDomain shopDomain)
        {
            Guid = guid;
            _unitOfWork = unitOfWork;
            _shopDomain = shopDomain;
        }

        public Guid OpenShop()
        {
            throw new BadStateException($"Tried to invoke OpenShop in GuestUser");
        }

        public Guid OpenShop(string shopName)
        {
            throw new BadStateException($"Tried to invoke OpenShop in GuestUser");
        }

        public void ReopenShop(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke ReopenShop in GuestUser");
        }

        public void CloseShop(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke CloseShop in GuestUser");
        }

        public void CloseShopPermanently(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke CloseShopPermanently in GuestUser");
        }

        public bool PurchaseCart(Guid shopGuid)
        {
            var bag = GetGuestBagAndCreateIfNeeded(shopGuid);

            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            //Can implement RollBack, purchase is given a Guid, shop.PurchaseCart returns a Guid,
            // if the user fails to pay later, we can delete the purchase and revert the shop quantities and cart content
            _shopDomain.ShoppingBagDomain.CheckDiscountPolicy(bag, shopGuid);
            if (!_shopDomain.PurchaseCart(shop, bag))
                return false;
            //External payment pay, if not true ---- rollback
            return true;
        }

        public bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in GuestUser");
        }

        public bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in GuestUser");
        }

        public bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in GuestUser");
        }

        public Guid AddProductToShop(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShop in GuestUser");
        }

        public void EditProductInShop( Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditProductInShop in GuestUser");
        }

        public bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromShop in GuestUser");
        }

        public bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            var bag = GetGuestBagAndCreateIfNeeded(shopGuid);
            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopGuid);
            var actualProduct = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            _shopDomain.ShoppingBagDomain.AddProductToCart(bag, shopGuid, actualProduct, quantity);
            return true;
        }

        public bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<bool> privileges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in GuestUser");
        }

        public bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in GuestUser");
        }

        public bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            var bag = GetGuestBagAndCreateIfNeeded(shopGuid);
            return _shopDomain.ShoppingBagDomain.EditProductInCart(bag, shopGuid, shopProductGuid, newAmount);
        }

        public bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            var bag = GetGuestBagAndCreateIfNeeded(shopGuid);
            return _shopDomain.ShoppingBagDomain.RemoveProductFromCart(bag, shopGuid, shopProductGuid);
        }

        public ICollection<ShopProduct> GetAllProductsInCart(Guid shopGuid)
        {
            var cart = _unitOfWork.BagRepository.GetShoppingCartAndCreateIfNeeded(Guid, shopGuid);
            return cart.GetAllProductsInCart();
        }

        public bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in GuestUser");
        }

        public bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in GuestUser");
        }

        public ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            var searcher = new ProductsSearcher(searchType, _unitOfWork);
            return searcher.Search(toMatch);
        }

        private ShoppingBag GetGuestBagAndCreateIfNeeded(Guid shopGuid)
        {
            return DomainData.GuestsBagsCollection.GetShoppingBagAndCreateIfNeeded(Guid);
        }

        public Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewPurchasePolicy in GuestUser");
        }

        public Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewDiscountPolicy in GuestUser");
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory()
        {
            return _unitOfWork.ShopRepository.Query().SelectMany(shop => shop.GetPurchaseHistory(Guid)).ToList();
        }

        public IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> GetUserBag()
        {
            var bag = GetGuestBagAndCreateIfNeeded(Guid);
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
    }
}
