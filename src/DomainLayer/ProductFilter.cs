using ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace DomainLayer
{
    /// <summary>
    /// Used for filtering product search.
    /// </summary>
    public class ProductFilter
    {
        Func<Product, bool> predicate;
        public ProductFilter(Func<Product, bool> pred)
        {
            this.predicate = pred;
        }

        public void ApplyFilter(List<Product> list)
        {
            List<Product> newList = new List<Product>();
            foreach (Product product in list)
            {
                if (predicate.Invoke(product))
                {
                    newList.Add(product);
                }
            }
            list = newList;// applying the side effects 
        }
    }
}
