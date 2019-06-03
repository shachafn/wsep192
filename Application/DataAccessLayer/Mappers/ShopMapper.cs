using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopMapper : IGenericMapper<Shop, ShopDAO>
    {
        BaseMapingManager _baseMapingManager;

        public ShopMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<Shop, ShopDAO>(this);
        }

        public object Map(object from)
        {
            throw new NotImplementedException();
        }

        ShopDAO IGenericMapper<Shop, ShopDAO>.Map(Shop fromObject)
        {
            ShopDAO mappedObject = new ShopDAO();
            mappedObject.Guid = fromObject.GetGuid();
            foreach (ShopOwner owner in fromObject.Owners)
                mappedObject.Owners.Add(_baseMapingManager.Map<ShopOwner,ShopOwnerDAO>(owner));  //map owner in ShopOwnerMapper
            foreach (ShopOwner manager in fromObject.Managers)
                mappedObject.Owners.Add(_baseMapingManager.Map<ShopOwner, ShopOwnerDAO>(manager));  //map owner in ShopOwnerMapper
            foreach (ShopProduct shopProduct in fromObject.ShopProducts)
                mappedObject.ShopProducts.Add(_baseMapingManager.Map<ShopProduct, ShopProductDAO>(shopProduct)); //map shopProduct with shopProductmaapper
            switch (fromObject.ShopState)
            {
                case Shop.ShopStateEnum.Active:
                    mappedObject.ShopState = ShopDAO.ShopStateEnum.Active;
                    break;
                case Shop.ShopStateEnum.Closed:
                    mappedObject.ShopState = ShopDAO.ShopStateEnum.Closed;
                    break;
                case Shop.ShopStateEnum.PermanentlyClosed:
                    mappedObject.ShopState = ShopDAO.ShopStateEnum.PermanentlyClosed;
                    break;
            }
            mappedObject.ShopName = fromObject.ShopName;
            return mappedObject;
        }
    }
}
