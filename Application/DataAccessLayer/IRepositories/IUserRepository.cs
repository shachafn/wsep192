using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.DAOs;

namespace DataAccessLayer.IRepositories
{
    interface IUserRepository : IRepositoryBase<BaseUserDAO>
    {
    }
}
