using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopOwnerMapper : IGenericMapper<ShopOwner, ShopOwnerDAO>
    {
        ShopOwnerDAO IGenericMapper<ShopOwner, ShopOwnerDAO>.Map(ShopOwner fromObject)
        {
            ShopOwnerDAO toReturn = new ShopOwnerDAO();
            toReturn.OwnerGuid = fromObject.OwnerGuid;
            toReturn.AppointerGuid = fromObject.AppointerGuid;
            toReturn.OwnerBaseUserGuid = fromObject.OwnerGuid;
            toReturn.ShopGuid = fromObject.ShopGuid;
            return toReturn;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
