using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Models.SellerModels
{
    public class EditShopProductModel
    {
        public Guid Guid { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

    }
}
