using ApplicationCore.Entities.Users;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.IRepositories;
using ApplicationCore.Mapping;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<BaseUserDAO>, IUserRepository
    {
        readonly BaseMapingManager _baseMapingManager;
        public UserRepository(ApplicationContext context, BaseMapingManager baseMapingManager) : base(context)
        {
            _baseMapingManager = baseMapingManager;
        }

        public void Create(BaseUser entity)
        {
            var dto = _baseMapingManager.Map<BaseUser, BaseUserDAO>(entity);
            base.Create(dto);
        }

        public void Delete(BaseUser entity)
        {
            var dto = _baseMapingManager.Map<BaseUser, BaseUserDAO>(entity);
            base.Delete(dto);
        }
        public override void DeleteAll()
        {
            base.Context.Users.RemoveRange(base.Context.Users);
        }

        public IQueryable<BaseUser> FindByCondition(Expression<Func<BaseUser, bool>> expression)
        {
            throw new NotImplementedException("DEPRECATED, USE FindAll and query it.");
        }

        public void Update(BaseUser entity)
        {
            var dto = _baseMapingManager.Map<BaseUser, BaseUserDAO>(entity);
            base.Update(dto);
        }

        IQueryable<BaseUser> IRepositoryBase<BaseUser>.FindAll()
        {
            return base.FindAll().Select(b => _baseMapingManager.Map<BaseUserDAO,BaseUser>(b));
        }
    }
}
