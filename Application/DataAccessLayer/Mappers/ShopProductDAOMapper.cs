using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopProductDAOMapper : IGenericMapper<ShopProductDAO, ShopProduct>
    {
        ShopProduct IGenericMapper<ShopProductDAO, ShopProduct>.Map(ShopProductDAO fromObject)
        {
            ShopProduct toReturn = new ShopProduct();
            toReturn.Guid = fromObject.Id;
            toReturn.Price = fromObject.Price;
            toReturn.Quantity = fromObject.Quantity;
            toReturn.Product = fromObject.Product;
            return toReturn;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
