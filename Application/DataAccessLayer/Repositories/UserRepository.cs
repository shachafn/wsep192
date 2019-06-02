using ApplicationCore.Entities.Users;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;


namespace DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<BaseUserDAO>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(BaseUser entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BaseUser entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BaseUser> FindByCondition(Expression<Func<BaseUser, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(BaseUser entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<BaseUser> IRepositoryBase<BaseUser>.FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
