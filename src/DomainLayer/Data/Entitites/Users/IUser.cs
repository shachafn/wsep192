using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Data.Entitites.Users
{
    public interface IUser
    {
        ICollection<ShoppingBag> GetShoppingHistory();

        void PurchaseBag();

        Guid OpenShop();

        bool CheckPass(string password);

        bool RemoveUser(Guid userToRemoveGuid);

        bool ConnectToPaymentSystem();

        bool ConnectToSupplySystem();

        Guid AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity);

        void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);

        bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid);

        bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity);

        bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid);

        bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);

        bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid);

        bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount);

        bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid);

        ICollection<Guid> GetAllProductsInCart(Guid shopGuid);

        bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid);

    }
}
