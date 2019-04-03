using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingBag ShoppingBag { get; set; }

        public Shop Shop { get; set; }

        public ICollection<ShopProduct> PurchasedProducts { get; set; }

        public void AddProduct(ShopProduct newShopProduct)
        {
            throw new NotImplementedException();
        }
        public void RemoveProduct(Guid productGuid)
        {
            throw new NotImplementedException();
        }
        public void EditProduct(Guid productGuid, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public ICollection<ShopProduct> GetAllProducts()
        {
            throw new NotImplementedException();
        }
        public double CalculateTotalPrice()
        {
            throw new NotImplementedException();
        }
        public bool PurchaseCart(User user)
        {
            throw new NotImplementedException();
        }
    }
}
