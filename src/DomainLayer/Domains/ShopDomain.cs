﻿using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Events;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Policies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Domains
{
    public class ShopDomain
    {
        public ShoppingBagDomain ShoppingBagDomain;
        protected IUnitOfWork _unitOfWork;
        protected ILogger<ShopDomain> _logger;

        public ShopDomain(ShoppingBagDomain shoppingCartDomain, IUnitOfWork unitOfWork, ILogger<ShopDomain> logger)
        {
            ShoppingBagDomain = shoppingCartDomain;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Close(Shop shop)
        {
            //throw new NotImplementedException();
            shop.VerifyShopIsActive(); ////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            shop.ShopState = Shop.ShopStateEnum.Closed;
            _unitOfWork.ShopRepository.Update(shop);
        }
        public void ClosePermanently(Shop shop)
        {
            shop.ShopState = Shop.ShopStateEnum.PermanentlyClosed;
            _unitOfWork.ShopRepository.Update(shop);
        }
        public void Reopen(Shop shop)
        {
            shop.ShopState = Shop.ShopStateEnum.Active;
            _unitOfWork.ShopRepository.Update(shop);
        }

        public Guid AddProductToShop(Shop shop, Guid userGuid, Product product, double price, int quantity)
        {
            var newShopProduct = new ShopProduct(product, price, quantity);
            shop.AddProductToShop(newShopProduct);
            _unitOfWork.ShopRepository.Update(shop);
            return newShopProduct.Guid;
        }
        public void EditProductInShop(Shop shop, Guid userGuid, Guid shopProductGuid, double newPrice, int newQuantity)
        {
            var toEdit = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            toEdit.Price = newPrice;
            toEdit.Quantity = newQuantity;
            _unitOfWork.ShopRepository.Update(shop);
        }

        public void RemoveProductFromShop(Shop shop, Guid userGuid, Guid shopProductGuid)
        {
            var toRemove = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            //Need to remove from all users' cart in this shop first, to not break constraint
            shop.ShopProducts.Remove(toRemove);
            ShoppingBagDomain.RemoveProductFromAllCarts(shopProductGuid);
            _unitOfWork.ShopRepository.Update(shop);
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(Shop shop, Guid userGuid)
        {
            ICollection<Tuple<Guid, ShopProduct, int>> toReturn = new List<Tuple<Guid, ShopProduct, int>>();
            foreach (Tuple<Guid, ShopProduct, int> purchase in shop.UsersPurchaseHistory)
            {
                if (userGuid.Equals(purchase.Item1))
                    toReturn.Add(new Tuple<Guid, ShopProduct, int>(shop.Guid, purchase.Item2, purchase.Item3));
            }
            _unitOfWork.ShopRepository.Update(shop);
            return toReturn;
        }

        public bool CascadeRemoveShopOwner(Shop shop, Guid userGuid, Guid ownerToRemoveGuid)
        {
            var removedOwners = shop.CascadeRemoveShopOwner(userGuid, ownerToRemoveGuid);
            foreach(var removedOwner in removedOwners)
            {
                var newEvent = new RemovedOwnerEvent(removedOwner, ownerToRemoveGuid, shop.Guid);
                newEvent.SetMessages(_unitOfWork);
                UpdateCenter.RaiseEvent(newEvent);
            }
            _unitOfWork.ShopRepository.Update(shop);
            return true;
        }

        public void AddShopOwner(Shop shop, Guid userGuid, Guid newOwnerGuid, string appointerUsername)
        {
            int signatures_required = shop.Owners.Count() + 1; //+1 For Creator
            if (shop.candidate == null && signatures_required > 1) // a new candidate
            {
                var newCandidate = new OwnerCandidate(newOwnerGuid, shop.Guid, userGuid, signatures_required, appointerUsername);
                shop.candidate = newCandidate;
            }
            else if (shop.candidate != null) // there is a candidate, sign it if possible
            {
                var candidate = shop.candidate;
                if (candidate.signature_target - candidate.Signatures.Count() == 1) // last sign , promote the candidate to a shop owner
                {
                    var newOwner = new ShopOwner(newOwnerGuid, candidate.AppointerGuid, shop.Guid);
                    shop.Owners.Add(newOwner);
                    shop.candidate = null;
                }
                else if (!candidate.Signatures.Values.Contains(userGuid)) //if not already signed , sign the candidate
                {
                    candidate.Signatures.Add(appointerUsername, userGuid);
                }
            }
            else// in the case: adding the second shopowner
            {
                var newOwner = new ShopOwner(newOwnerGuid, userGuid, shop.Guid);
                shop.Owners.Add(newOwner);
            }

            _unitOfWork.ShopRepository.Update(shop);
        }

        public bool RemoveShopManager(Shop shop, Guid userGuid, Guid managerToRemoveGuid)
        {
            var result = shop.RemoveShopManager(userGuid, managerToRemoveGuid);
            _unitOfWork.ShopRepository.Update(shop);
            return result;
        }

        public void AddShopManager(Shop shop, Guid userGuid, Guid newManagaerGuid, List<bool> priviliges)
        {
            var newManager = new ShopOwner(newManagaerGuid, userGuid, shop.Guid, priviliges);
            shop.AddShopManager(newManager);
            _unitOfWork.ShopRepository.Update(shop);
        }

        public ShopOwner GetOwner(Shop shop, Guid userGuid)
        {
            var result = shop.GetOwner(userGuid);
            _unitOfWork.ShopRepository.Update(shop);
            return result;
        }
        public bool IsOwner(Shop shop, Guid userGuid)
        {
            if (shop.Creator.OwnerGuid.Equals(userGuid))
                return true;
            return shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid)) != null;
        }

        public bool AddOwner(Shop shop, Guid appointerGuid, Guid newOwnerGuid)
        {
            var result = false;
            int signatures_required = shop.Owners.Count() + 1;//+1 for the shop creator
            if (shop.candidate == null && signatures_required > 1) // a new candidate
            {
                var newCandidate = new OwnerCandidate(newOwnerGuid, shop.Guid, appointerGuid, signatures_required, _unitOfWork.BaseUserRepository.GetUsername(appointerGuid));
                shop.candidate = newCandidate;
            }
            else if (shop.candidate != null) // there is a candidate, sign it if possible
            {
                var candidate = shop.candidate;
                if (candidate.signature_target - candidate.Signatures.Count() == 1) // last sign , promote the candidate to a shop owner
                {
                    var newOwner = new ShopOwner(newOwnerGuid, candidate.AppointerGuid, shop.Guid);
                    shop.Owners.Add(newOwner);
                    shop.candidate = null;
                }
                else if (!candidate.Signatures.Values.Contains(appointerGuid)) //if not already signed , sign the candidate
                {
                    candidate.Signatures.Add(_unitOfWork.BaseUserRepository.GetUsername(appointerGuid), appointerGuid);
                }
            }
            else// in the case: adding the second shopowner
            {
                var newOwner = new ShopOwner(newOwnerGuid, appointerGuid, shop.Guid);
                shop.Owners.Add(newOwner);
            }
            result = true;
            _unitOfWork.ShopRepository.Update(shop);
            return result;
        }

        public bool RemoveOwner(Shop shop, Guid toRemoveOwnerGuid)
        {
            var ownerToRemove = shop.GetOwner(toRemoveOwnerGuid);
            if (ownerToRemove.OwnerGuid.Equals(shop.Creator.OwnerGuid))
                return false;
            foreach (var otherOwner in shop.Owners)
            {
                if (otherOwner.AppointerGuid.Equals(toRemoveOwnerGuid))
                {
                    RemoveOwner(shop, otherOwner.OwnerGuid);
                    var newEvent = new RemovedOwnerEvent(otherOwner.OwnerGuid, toRemoveOwnerGuid, shop.Guid);
                    newEvent.SetMessages(_unitOfWork);
                    UpdateCenter.RaiseEvent(newEvent);
                }
            }
            shop.Owners.Remove(ownerToRemove);// remove the owner from the owners list
            _unitOfWork.ShopRepository.Update(shop);
            return true;
        }

        public Guid AddNewPurchasePolicy(Shop shop, IPurchasePolicy newPurchasePolicy)
        {
            if (shop.PurchasePolicies == null)
            {
                shop.PurchasePolicies = new List<IPurchasePolicy>();
            }
            shop.PurchasePolicies.Add(newPurchasePolicy);
            _unitOfWork.ShopRepository.Update(shop);
            return newPurchasePolicy.Guid;
        }

        public Guid AddNewDiscountPolicy(Shop shop, IDiscountPolicy newDiscountPolicy)
        {
            if (shop.DiscountPolicies == null)
            {
                shop.DiscountPolicies = new List<IDiscountPolicy>();
            }
            shop.DiscountPolicies.Add(newDiscountPolicy);
            _unitOfWork.ShopRepository.Update(shop);
            return newDiscountPolicy.Guid;
        }

        public bool PurchaseCart(Shop shop, ShoppingBag bag)
        {
            var cart = bag.GetShoppingCartAndCreateIfNeededForGuestOnlyOrInBagDomain(shop.Guid);
            bool canPurchaseCart = true;

            foreach (var productAndAmountBought in cart.PurchasedProducts)
            {
                var userGuid = cart.UserGuid;
                BaseUser user;
                try
                {
                    user = _unitOfWork.BaseUserRepository.FindByIdOrNull(userGuid);
                }
                catch (Exception)
                {
                    user = null;
                }
                //check purchase policies
                var quantity = productAndAmountBought.Item2;
                foreach (IPurchasePolicy policy in shop.PurchasePolicies)
                    if (!policy.CheckPolicy(cart, productAndAmountBought.Item1.Guid, quantity, user, _unitOfWork) && !policyInCompound(shop,policy))
                        canPurchaseCart = false;
            }
            if (!canPurchaseCart)
            {
                //_unitOfWork.ShopRepository.Update(shop);
                //_unitOfWork.BagRepository.Update(bag);
                return false;
            }

            foreach (var productAndAmountBought in cart.PurchasedProducts)
            {
                var userGuid = cart.UserGuid;

                var actualProduct = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(productAndAmountBought.Item1.Guid));
                //decrease stock quantity if it wasnt a discount record

                var quantity = productAndAmountBought.Item2;
                if (actualProduct != null)
                {
                    shop.UsersPurchaseHistory.Add(new Tuple<Guid, ShopProduct, int>(userGuid, actualProduct, quantity));
                    actualProduct.Quantity -= quantity;
                }
                else
                {
                    shop.UsersPurchaseHistory.Add(new Tuple<Guid, ShopProduct, int>(userGuid, productAndAmountBought.Item1, quantity));
                }

            }
            double total_price = cart.PurchasedProducts.Aggregate(0d, (total, p) => total += Convert.ToDouble(p.Item1.Price) * p.Item2);
            var newEvent = new PurchasedCartEvent(cart.UserGuid, cart.ShopGuid, total_price);
            newEvent.SetMessages(_unitOfWork);
            UpdateCenter.RaiseEvent(newEvent);
            ShoppingBagDomain.PurchaseCart(bag, shop.Guid);
            _unitOfWork.ShopRepository.Update(shop);
            return true;
        }
        private bool policyInCompound(Shop shop, IPurchasePolicy policy)
        {
            foreach(IPurchasePolicy p in shop.PurchasePolicies)
            {
                if(p.GetType() == typeof(CompositePurchasePolicy))
                {
                    if (((CompositePurchasePolicy)p).PurchasePolicy1.Guid.CompareTo(policy.Guid) == 0 || ((CompositePurchasePolicy)p).PurchasePolicy2.Guid.CompareTo(policy.Guid)==0)
                        return true;
                }
            }
            return false;
        }
        public double GetCartPrice(Shop shop, ShoppingCart cart)
        {
            double total_price = cart.PurchasedProducts.Aggregate(0d, (total, p) => total += Convert.ToDouble(p.Item1.Price) * p.Item2);
            return total_price;
        }

    }
}
