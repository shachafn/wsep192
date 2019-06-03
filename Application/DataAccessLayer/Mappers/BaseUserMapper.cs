using ApplicationCore.Entities.Users;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class BaseUserMapper : IGenericMapper<BaseUser, BaseUserDAO>
    {
        BaseMapingManager _baseMapingManager;

        public BaseUserMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<BaseUser, BaseUserDAO>(this);
        }
        BaseUserDAO IGenericMapper<BaseUser, BaseUserDAO>.Map(BaseUser fromObject)
        {
            BaseUserDAO mappedObject = new BaseUserDAO(fromObject);
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
