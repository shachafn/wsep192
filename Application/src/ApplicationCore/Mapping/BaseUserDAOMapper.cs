using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    class BaseUserDAOMapper : IGenericMapper<BaseUserDAO, BaseUser>
    {
        BaseUser IGenericMapper<BaseUserDAO, BaseUser>.Map(BaseUserDAO fromObject)
        {
            BaseUser mappedObject = new BaseUser(fromObject.Username, fromObject.Get_hash(), fromObject.IsAdmin);
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
