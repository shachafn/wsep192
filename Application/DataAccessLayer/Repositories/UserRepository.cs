using ApplicationCore.Entities.Users;
using DataAccessLayer.DAOs;
using DataAccessLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<BaseUserDAO>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
