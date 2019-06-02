using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentaitionLayer.Models.BuyerModels
{
    public class ShopProductBuyModel
    {
        public Guid Id { get; set; } //items guid
        public string Name { get; set; }
        public string Category { get; set; }
        public ICollection<string> Keywords { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int addQuantity { get; set; }//how much to add to the cart
    }
}
