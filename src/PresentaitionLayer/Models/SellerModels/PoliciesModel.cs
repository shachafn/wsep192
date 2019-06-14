using DomainLayer.Policies;
using System.Collections.Generic;

namespace PresentaitionLayer.Models.SellerModels
{
    public class PoliciesModel
    {
        public PoliciesModel()
        {

        }
        public string name { get; set; }
        public ICollection<IPurchasePolicy> PurchasePolicies { get; set; }
        public ICollection<IDiscountPolicy> DiscountPolicies { get; set; }
    }
}
