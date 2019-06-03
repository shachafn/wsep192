using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopProductMapper : IGenericMapper<ShopProduct, ShopProductDAO>
    {
        BaseMapingManager _baseMapingManager;

        public ShopProductMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ShopProduct, ShopProductDAO>(this);
        }

        ShopProductDAO IGenericMapper<ShopProduct, ShopProductDAO>.Map(ShopProduct fromObject)
        {
            ShopProductDAO shopProductDAO = new ShopProductDAO();
            shopProductDAO.Id = fromObject.GetGuid();
            shopProductDAO.ProductGuid = fromObject.Product.GetGuid();
            shopProductDAO.Quantity = fromObject.Quantity;
            shopProductDAO.Price = fromObject.Price;
            shopProductDAO.Product = _baseMapingManager.Map<Product, ProductDAO>(fromObject.Product);
            return shopProductDAO;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
