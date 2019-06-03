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
        ShopProductDAO IGenericMapper<ShopProduct, ShopProductDAO>.Map(ShopProduct fromObject)
        {
            ShopProductDAO shopProductDAO = new ShopProductDAO();
            shopProductDAO.Id = fromObject.GetGuid();
            shopProductDAO.ProductGuid = fromObject.Product.GetGuid();
            shopProductDAO.Quantity = fromObject.Quantity;
            shopProductDAO.Price = fromObject.Price;
            return shopProductDAO;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
