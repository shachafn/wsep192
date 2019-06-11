using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentaitionLayer.Models.BuyerModels
{
    public class CheckoutModel
    {
       public  IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> _products { get; set; }

        public CheckoutModel(IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> products)
        {
            _products = products;
        }
    }
}
