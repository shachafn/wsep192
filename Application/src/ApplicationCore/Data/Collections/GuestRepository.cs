using ApplicationCore.Entities.Users;
using ApplicationCore.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Data.Collections
{
    public class GuestRepository : IRepositoryBase<IUser>
    {
        private static GuestsEntityCollection _loggedInUsersEntityCollection = new GuestsEntityCollection();

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

        public IQueryable<IUser> FindAll()
        {
            return _loggedInUsersEntityCollection.Values.AsQueryable();
        }

        public IQueryable<IUser> FindByCondition(Expression<Func<IUser, bool>> expression)
        {
            return _loggedInUsersEntityCollection.AsQueryable().Where(expression);
        }

        public void Update(IUser entity)
        {
            _loggedInUsersEntityCollection[entity.Guid] = entity;
        }
    }
}
