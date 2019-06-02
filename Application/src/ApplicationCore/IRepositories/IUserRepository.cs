using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore.IRepositories
{
    public interface IUserRepository : IRepositoryBase<BaseUser>
    {
    }
}
