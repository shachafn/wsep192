using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestsUtils
{
    public class MockBaseUserRepository : IBaseUserRepository
    {
        private ICollection<BaseUser> BaseUsers { get; set; } = new List<BaseUser>();

        public void Add(BaseUser entity)
        {
            BaseUsers.Add(entity);
        }

        public void Clear()
        {
            BaseUsers.Clear();
        }

        public void Delete(BaseUser entity)
        {
            BaseUsers.Remove(entity);
        }

        public ICollection<BaseUser> FetchAll()
        {
            return BaseUsers.ToList();
        }

        public BaseUser FindByIdOrNull(Guid id)
        {
            return BaseUsers.FirstOrDefault(b => b.Guid.Equals(id));
        }

        public BaseUser GetByUsername(string username)
        {
            return BaseUsers.FirstOrDefault(b => b.Username.Equals(username));
        }

        public Guid GetUserGuidByUsername(string username)
        {
            return BaseUsers.FirstOrDefault(b => b.Username.Equals(username)).Guid;
        }

        public string GetUsername(Guid guid)
        {
            return BaseUsers.FirstOrDefault(b => b.Guid.Equals(guid)).Username;
        }

        public bool IsUserExistsByGuid(Guid guid)
        {
            return BaseUsers.Any(b => b.Guid.Equals(guid));
        }

        public bool IsUserExistsByUsername(string username)
        {
            return BaseUsers.Any(b => b.Username.Equals(username));
        }

        public IQueryable<BaseUser> Query()
        {
            return BaseUsers.AsQueryable();
        }

        public void Update(BaseUser entity)
        {
            BaseUsers.Remove(entity);
            BaseUsers.Add(entity);
        }
    }
}