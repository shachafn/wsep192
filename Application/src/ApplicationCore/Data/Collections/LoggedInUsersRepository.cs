using ApplicationCore.Entities.Users;
using ApplicationCore.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Data.Collections
{
    class LoggedInUsersRepository : IRepositoryBase<IUser>
    {
        private static LoggedInUsersEntityCollection _loggedInUsersEntityCollection = new LoggedInUsersEntityCollection();

        public void Create(IUser entity)
        {
            _loggedInUsersEntityCollection.Add(entity.Guid, entity);
        }

        public void Delete(IUser entity)
        {
            _loggedInUsersEntityCollection.Remove(entity.Guid);
        }

        public void DeleteAll()
        {
            _loggedInUsersEntityCollection.Clear();

        }

        public void DeleteById(params Guid[] ids)
        {
            if (_loggedInUsersEntityCollection.ContainsKey(ids.First()))
                _loggedInUsersEntityCollection.Remove(ids.First());
        }

        public IQueryable<IUser> FindAll()
        {
            return _loggedInUsersEntityCollection.Values.AsQueryable();
        }

        public IQueryable<IUser> FindByCondition(Expression<Func<IUser, bool>> expression)
        {
            return _loggedInUsersEntityCollection.AsQueryable().Where(expression);
        }

        public IUser FindById(Guid id)
        {
            if (_loggedInUsersEntityCollection.ContainsKey(id))
                return _loggedInUsersEntityCollection[id];
            return null;
        }

        public void Update(IUser entity)
        {
            _loggedInUsersEntityCollection[entity.Guid] = entity;
        }
    }
}
