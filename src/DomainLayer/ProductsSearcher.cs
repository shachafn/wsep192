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
                        foreach (var shop in activeShops)
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
                        foreach (var shop in activeShops)
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
