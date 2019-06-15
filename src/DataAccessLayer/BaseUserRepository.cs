using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace DataAccessLayer
{
    internal class BaseUserRepository : BaseMongoRepository, IBaseUserRepository
    {
        private IContext context;

        public BaseUserRepository(IContext context) : base(context)
        {
            this.context = context;
        }

        public void Add(BaseUser entity)
        {
            base.Add<BaseUser>(entity, context.GetCurrentSession());
        }

        public void Delete(BaseUser entity)
        {
            base.Delete<BaseUser>(entity, context.GetCurrentSession());
        }

        public ICollection<BaseUser> FetchAll()
        {
            return base.FetchAll<BaseUser>(context.GetCurrentSession());
        }

        public BaseUser FindByIdOrNull(Guid guid)
        {
            return base.FindByIdOrNull<BaseUser>(guid, context.GetCurrentSession());
        }

        public IQueryable<BaseUser> Query()
        {
            return base.Query<BaseUser>(context.GetCurrentSession());
        }

        public void Update(BaseUser entity)
        {
            base.Update<BaseUser>(entity, context.GetCurrentSession());
        }

        /// <summary>
        /// Returns the Username of the user with the given guid
        /// </summary>
        public string GetUsername(Guid guid)
        {
            return FindByIdOrNull(guid).Username;
        }

        public BaseUser GetByUsername(string username)
        {
            return Query().FirstOrDefault(b => b.Username.Equals(username));
        }

        public Guid GetUserGuidByUsername(string username)
        {
            return GetByUsername(username).Guid;
        }

        public bool IsUserExistsByGuid(Guid guid)
        {
            return Query().Any(user => user.Guid.Equals(guid));
        }

        public bool IsUserExistsByUsername(string username)
        {
            return Query().Any(user => user.Username.Equals(username));
        }

        public void Clear()
        {
            throw new InvalidOperationException("Clear should only be used for tests.");
        }
    }
}