using ApplicationCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer
{
    public class ProductsSearcher
    {
        private string _searchType;

        public ProductsSearcher(string searchType)
        {
            this._searchType = searchType;
        }

        public ICollection<Guid> Search(ICollection<string> toMatch)
        {
            switch (_searchType)
            {
                case "Name":
                    {
                        return DomainData.ShopsCollection
                            .SelectMany(shop => shop.ShopProducts)
                            .Where(prod => prod.Product.Name.ToLower().Contains(toMatch.First().ToLower()))
                            .Select(prod => prod.Guid)
                            .ToList();
                    }
                case "Category":
                    {
                        return DomainData.ShopsCollection
                            .SelectMany(shop => shop.ShopProducts)
                            .Where(prod => prod.Product.Category.ToLower().Equals(toMatch.First().ToLower()))
                            .Select(prod => prod.Guid)
                            .ToList();
                    }
                case "Keywords":
                    {
                        return DomainData.ShopsCollection
                            .SelectMany(shop => shop.ShopProducts)
                            .Where(prod => toMatch.All(keyword => prod.Product.Keywords.Contains(keyword)))
                            .Select(prod => prod.Guid)
                            .ToList();
                    }
                default:
                    return null;
            }
        }
    }
}
