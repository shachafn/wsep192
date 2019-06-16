using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;

namespace PresentaitionLayer.Models.BuyerModels
{
    public class CheckoutModel
    {
        public  IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> _products { get; set; }
        public IEnumerable<double> AfterDiscount { get; set; }

        public CheckoutModel(IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> products)
        {
            _products = products;
        }
    }
}
