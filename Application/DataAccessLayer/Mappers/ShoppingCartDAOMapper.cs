using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShoppingCartDAOMapper : IGenericMapper<ShoppingCartDAO, ShoppingCart>
    {
        BaseMapingManager _baseMapingManager;

        public ShoppingCartDAOMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ShoppingCartDAO, ShoppingCart>(this);
        }
        ShoppingCart IGenericMapper<ShoppingCartDAO, ShoppingCart>.Map(ShoppingCartDAO fromObject)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.Guid = fromObject.cartGuid;
            shoppingCart.ShopGuid = fromObject.ShopGuid;
            shoppingCart.UserGuid = fromObject.UserGuid;
            foreach (RecordsPerCartDAO record in fromObject.RecordsGuids)
                shoppingCart.PurchasedProducts.Add(new Tuple<Guid, int>(record.ProductGuid,record.PurchasedQuantity)); //mapper of tuple and records.
            return shoppingCart;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
