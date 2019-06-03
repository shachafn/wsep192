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
        BaseMapingManager _baseMapingManager;

        public ShoppingCartMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ShoppingCart, ShoppingCartDAO>(this);
        }
        ShoppingCartDAO IGenericMapper<ShoppingCart, ShoppingCartDAO>.Map(ShoppingCart fromObject)
        {
            ShoppingCartDAO mappedObject = new ShoppingCartDAO();
            mappedObject.cartGuid = fromObject.GetGuid();
            mappedObject.UserGuid = fromObject.UserGuid;
            mappedObject.ShopGuid = fromObject.ShopGuid;
            foreach(var record in fromObject.PurchasedProducts)
            {
                mappedObject.RecordsGuids.Add(new RecordsPerCartDAO()
                {
                    CartGuid = mappedObject.cartGuid,
                    ProductGuid = record.Item1,
                    PurchasedQuantity = record.Item2
                }); //mapping pair of guid and int to RecordDAO
            }
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
