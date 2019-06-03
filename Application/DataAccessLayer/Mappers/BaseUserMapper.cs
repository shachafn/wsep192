using ApplicationCore.Entities.Users;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    class BaseUserMapper : IGenericMapper<BaseUser, BaseUserDAO>
    {
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
