﻿using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopOwnerDAOMapper : IGenericMapper<ShopOwnerDAO, ShopOwner>
    {
        BaseMapingManager _baseMapingManager;

        public ShopOwnerDAOMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ShopOwnerDAO, ShopOwner>(this);
        }
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
