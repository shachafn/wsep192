using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{

    public class CompositePurchasePolicy : IPurchasePolicy
    {
        public Guid Guid { get; set; }
        public IPurchasePolicy PurchasePolicy1 { get; set; }
        public ILogicOperator Operator { get; set; }
        public IPurchasePolicy PurchasePolicy2 { get; set; }
        public string Description { get; set; }


        public CompositePurchasePolicy(IPurchasePolicy purchasePolicy1, ILogicOperator @operator, IPurchasePolicy purchasePolicy2, string description)
        {
            Guid = Guid.NewGuid();
            PurchasePolicy1 = purchasePolicy1;
            Operator = @operator;
            PurchasePolicy2 = purchasePolicy2;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            return Operator.Operate(PurchasePolicy1.CheckPolicy(cart, productGuid, quantity, user, unitOfWork), PurchasePolicy2.CheckPolicy(cart, productGuid, quantity, user, unitOfWork));
        }
    }
}
