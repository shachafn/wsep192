using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShoppingCartMapper : IGenericMapper<ShoppingCart, ShoppingCartDAO>
    {
        ShoppingCartDAO IGenericMapper<ShoppingCart, ShoppingCartDAO>.Map(ShoppingCart fromObject)
        {
            throw new NotImplementedException();
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
