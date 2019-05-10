using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Models.SellerModels
{
    public class ShopManageIndexModel
    {
        public ShopManageIndexModel(Shop shop)
        {
            Shop = shop;

        }

        public ShopManageIndexModel()
        {

        }
        public Shop Shop { get; set; }

    }
}
