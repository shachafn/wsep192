using ApplicationCore.Data;
using ApplicationCore.Entitites;
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

        public ICollection<Tuple<ShopProduct, Guid>> Search(ICollection<string> toMatch)
        {
            switch (_searchType)
            {
                case "Name":
                    {
                        //Product and shopGuid
                        ICollection<Tuple<ShopProduct, Guid>> output = new List<Tuple<ShopProduct, Guid>>();
                        foreach (var shop in DomainData.ShopsCollection.Where(s=>s.ShopState.Equals(Shop.ShopStateEnum.Active)))
                        {
                            var filteredProducts = shop.ShopProducts.
                                Where(product => product.Product.Name.ToLower().Contains(toMatch.First().ToLower()));

                            foreach (var product in filteredProducts)
                                output.Add(new Tuple<ShopProduct, Guid>(product, shop.Guid));
                        }
                        return output;
                    }
                case "Category":
                    {
                        ICollection<Tuple<ShopProduct, Guid>> output = new List<Tuple<ShopProduct, Guid>>();
                        foreach (var shop in DomainData.ShopsCollection.Where(s => s.ShopState.Equals(Shop.ShopStateEnum.Active)))
                        {
                            var filteredProducts = shop.ShopProducts.
                                Where(prod => prod.Product.Category.ToLower().Equals(toMatch.First().ToLower()));

                            foreach (var product in filteredProducts)
                                output.Add(new Tuple<ShopProduct, Guid>(product, shop.Guid));
                        }
                        return output;
                    }
                case "Keywords":
                    {
                        ICollection<Tuple<ShopProduct, Guid>> output = new List<Tuple<ShopProduct, Guid>>();
                        foreach (var shop in DomainData.ShopsCollection.Where(s => s.ShopState.Equals(Shop.ShopStateEnum.Active)))
                        {
                            var filteredProducts = shop.ShopProducts.
                                Where(prod => toMatch.All(keyword => prod.Product.Keywords.Contains(keyword)));

                            foreach (var product in filteredProducts)
                                output.Add(new Tuple<ShopProduct, Guid>(product, shop.Guid));
                        }
                        return output;
                    }
                default:
                    return null;
            }
        }
    }
}
