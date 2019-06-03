using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShoppingBagMapper : IGenericMapper<ShoppingBag, ShoppingBagDAO>
    {
        ShoppingBagDAO IGenericMapper<ShoppingBag, ShoppingBagDAO>.Map(ShoppingBag fromObject)
        {
            ShoppingBagDAO toReturn = new ShoppingBagDAO();
            toReturn.UserGuid = fromObject.UserGuid;
            toReturn.BagGuid = fromObject.UserGuid;
            foreach (ShoppingCart cart in fromObject.ShoppingCarts)
                toReturn.ShoppingCarts.Add(null); //mapper of cart to cartDAO
            return toReturn;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
