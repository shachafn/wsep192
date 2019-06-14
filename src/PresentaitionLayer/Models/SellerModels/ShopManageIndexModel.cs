using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;

namespace PresentaitionLayer.Models.SellerModels
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
        public string CreatorName { get; set; }
        public List<(string, string, Guid)> Owners { get; set;}
        public List<(string, string, string, Guid)> Managers { get; set; } // each tuple is owner name, appointer name, and and index for retriving  permissions
        public (string, string) OwnerCandidate { get; set; }
    }
}
