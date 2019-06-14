using System;
using static ApplicationCore.Entitites.Shop;

namespace PresentaitionLayer.Models.AdminModels
{
    public class AdminShopDisplayModel
    {
        public Guid Guid { get; set; }
        public Guid CreatorGuid { get; set; }
        public ShopStateEnum State { get; set; }
    }
}
