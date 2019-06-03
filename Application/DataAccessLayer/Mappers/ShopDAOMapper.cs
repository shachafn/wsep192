using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    class ShopDAOMapper : IGenericMapper<ShopDAO, Shop>
    {
        Shop IGenericMapper<ShopDAO, Shop>.Map(ShopDAO fromObject)
        {
            Shop mappedObject = new Shop();
            mappedObject.Guid = fromObject.Guid;
            foreach (ShopOwnerDAO owner in fromObject.Owners)
                mappedObject.Owners.Add(null);  //map owner in ShopOwnerMapperDAO
            foreach (ShopOwnerDAO manager in fromObject.Managers)
                mappedObject.Owners.Add(null);  //map owner in ShopOwnerMapperDAO
            foreach (ShopProductDAO shopProduct in fromObject.ShopProducts)
                mappedObject.ShopProducts.Add(null); //map shopProduct with shopProductmaapperDAO
            switch (fromObject.ShopState)
            {
                case ShopDAO.ShopStateEnum.Active:
                    mappedObject.ShopState = Shop.ShopStateEnum.Active;
                    break;
                case ShopDAO.ShopStateEnum.Closed:
                    mappedObject.ShopState = Shop.ShopStateEnum.Closed;
                    break;
                case ShopDAO.ShopStateEnum.PermanentlyClosed:
                    mappedObject.ShopState = Shop.ShopStateEnum.PermanentlyClosed;
                    break;
            }
            mappedObject.ShopName = fromObject.ShopName;
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
