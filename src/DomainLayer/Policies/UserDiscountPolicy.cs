using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace DomainLayer.Policies
{
    public class UserDiscountPolicy : IDiscountPolicy
    {
        private Predicate<BaseUser> UserConstraint;
        //private Func<ShoppingCart,ShoppingCart> ChangeCartBySail; //Returns updated
        public UserDiscountPolicy()
        {
            
        }

        //Look at it when you'll need Func generator
        /*public async System.Threading.Tasks.Task<Func<int, int>> incrementFuncAsync()
        {
            var body = "x => x+1";
            var options = ScriptOptions.Default.AddReferences(typeof(int).Assembly);
            Func<int, int> discountFilterExpression = await CSharpScript.EvaluateAsync<Func<int, int>>(body, options);
            return discountFilterExpression;
        }*/

        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            throw new NotImplementedException();
        }
    }

}