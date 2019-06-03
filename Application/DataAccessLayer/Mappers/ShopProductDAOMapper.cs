using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopProductDAOMapper : IGenericMapper<ShopProductDAO, ShopProduct>
    {
        BaseMapingManager _baseMapingManager;

        public ShopProductDAOMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ShopProductDAO, ShopProduct>(this);
        }

        ShopProduct IGenericMapper<ShopProductDAO, ShopProduct>.Map(ShopProductDAO fromObject)
        {
            ShopProduct toReturn = new ShopProduct();
            toReturn.Guid = fromObject.Id;
            toReturn.Price = fromObject.Price;
            toReturn.Quantity = fromObject.Quantity;
            toReturn.Product = _baseMapingManager.Map<ProductDAO, Product>(fromObject.Product);
            return toReturn;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
