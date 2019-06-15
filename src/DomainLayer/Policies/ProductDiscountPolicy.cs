using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Operators;
using System;


namespace DomainLayer.Policies
{
    class ProductDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; private set; }
        private Guid ProductGuid { get; }
        private IArithmeticOperator Operator { get; }
        private int ExpectedQuantitiy { get; }
        public int DiscountPercentage { get; set; }
        public string Description { get; }


        public ProductDiscountPolicy(Guid productGuid, IArithmeticOperator @operator, int expectedQuantitiy, int discountPercentage, string description)
        {

            Guid = Guid.NewGuid();
            ProductGuid = productGuid;
            Operator = @operator;
            ExpectedQuantitiy = expectedQuantitiy;
            DiscountPercentage = discountPercentage;
            Description = description;
        }



        //Discount by percentage only!
        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            return productGuid.CompareTo(ProductGuid) == 0 && Operator.IsValid(ExpectedQuantitiy, quantity);
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity
            , BaseUser user, IUnitOfWork unitOfWork)
        {
            if (CheckPolicy(cart, productGuid, quantity, user, unitOfWork))
            {
                Product p = null;
                Shop s = unitOfWork.ShopRepository.FindByIdOrNull(cart.ShopGuid);
                double shopProductPrice = 0;
                foreach (ShopProduct shopProduct in s.ShopProducts)
                {
                    if (shopProduct.Guid.CompareTo(productGuid) == 0)
                    {
                        p = shopProduct.Product;
                        shopProductPrice = shopProduct.Price;
                        break;
                    }
                }
                double discountValue = -((double)DiscountPercentage / 100.0) * shopProductPrice;
                if (discountValue == 0) return null;
                Product discountProduct = new Product("Discount - " + p.Name, "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProduct, discountValue, 1);
                return new Tuple<ShopProduct, int>(discountRecord, quantity);
            }
            return null;
        }
    }
}
