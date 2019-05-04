using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ApplicationCore.Entitites.Shop;

namespace PressentaitionLayer.Models.AdminModels
{
    public class AdminShopDisplayModel
    {
        public Guid Guid { get; set; }
        public Guid CreatorGuid { get; set; }
        public ShopStateEnum State { get; set; }
    }
}
