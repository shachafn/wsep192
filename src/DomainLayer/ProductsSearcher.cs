using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer
{
    public class ProductsSearcher
    {
        private string _searchType;
        IUnitOfWork _unitOfWork;

        public ProductsSearcher(string searchType, IUnitOfWork unitOfWork)
        {
            this._searchType = searchType;
            _unitOfWork = unitOfWork;
        }

        public ICollection<Tuple<ShopProduct, Guid>> Search(ICollection<string> toMatch)
        {
            var activeShops = _unitOfWork.ShopRepository.GetActiveShops();
            switch (_searchType)
            {
                case "Name":
                    {
                        //Product and shopGuid
                        ICollection<Tuple<ShopProduct, Guid>> output = new List<Tuple<ShopProduct, Guid>>();
                        foreach (var shop in activeShops)
                        {
                            foreach (var str in toMatch)
                            {
                                var maxDist = Math.Ceiling(str.Length * 0.3);
                                var filteredProducts = shop.ShopProducts.
                                    Where(product => product.Product.Name.ToLower().Contains(str.ToLower())
                                        || LevenshteinDistance(product.Product.Name.ToLower(), str.ToLower()) <= maxDist);
                                foreach (var product in filteredProducts)
                                    output.Add(new Tuple<ShopProduct, Guid>(product, shop.Guid));
                            }
                        }
                        return output;
                    }
                case "Category":
                    {
                        ICollection<Tuple<ShopProduct, Guid>> output = new List<Tuple<ShopProduct, Guid>>();
                        foreach (var shop in activeShops)
                        {
                            foreach (var str in toMatch)
                            {
                                var maxDist = Math.Ceiling(str.Length * 0.3);
                                var filteredProducts = shop.ShopProducts.
                                    Where(prod => prod.Product.Category.ToLower().Contains(str.ToLower())
                                        || LevenshteinDistance(prod.Product.Category.ToLower(), str.ToLower()) <= maxDist);

                                foreach (var product in filteredProducts)
                                    output.Add(new Tuple<ShopProduct, Guid>(product, shop.Guid));
                            }
                        }
                        return output;
                    }
                case "Keywords":
                    {
                        ICollection<Tuple<ShopProduct, Guid>> output = new List<Tuple<ShopProduct, Guid>>();
                        foreach (var shop in activeShops)
                        {
                            foreach (var str in toMatch)
                            {
                                var maxDist = Math.Ceiling(str.Length * 0.3);
                                var filteredProducts = shop.ShopProducts.
                                    Where(prod => toMatch.Any(keyword => prod.Product.Keywords.Contains(str)
                                        || LevenshteinDistance(keyword, str.ToLower()) <= maxDist));

                                foreach (var product in filteredProducts)
                                    output.Add(new Tuple<ShopProduct, Guid>(product, shop.Guid));
                            }
                        }
                        return output;
                    }
                default:
                    return null;
            }
        }
        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
