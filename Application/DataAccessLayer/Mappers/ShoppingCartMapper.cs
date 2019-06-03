using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShoppingCartMapper : IGenericMapper<ShoppingCart, ShoppingCartDAO>
    {
        ShoppingCartDAO IGenericMapper<ShoppingCart, ShoppingCartDAO>.Map(ShoppingCart fromObject)
        {
            ShoppingCartDAO mappedObject = new ShoppingCartDAO();
            mappedObject.cartGuid = fromObject.GetGuid();
            mappedObject.UserGuid = fromObject.UserGuid;
            mappedObject.ShopGuid = fromObject.ShopGuid;
            foreach(<Tuple<Guid, int>> record in fromObject.PurchasedProducts)
            {
                mappedObject.RecordsGuids.Add(null); //mapping pair of guid and int to RecordDAO
            }
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
