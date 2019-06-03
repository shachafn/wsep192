using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopOwnerDAOMapper : IGenericMapper<ShopOwnerDAO, ShopOwner>
    {
        ShopOwner IGenericMapper<ShopOwnerDAO, ShopOwner>.Map(ShopOwnerDAO fromObject)
        {
            ShopOwner toReturn = new ShopOwner();
            toReturn.OwnerGuid = fromObject.OwnerGuid;
            toReturn.AppointerGuid = fromObject.AppointerGuid;
            //toReturn.owner = fromObject.OwnerGuid;
            toReturn.ShopGuid = fromObject.ShopGuid;
            return toReturn;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
