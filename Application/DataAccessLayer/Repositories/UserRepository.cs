using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<BaseUser>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
