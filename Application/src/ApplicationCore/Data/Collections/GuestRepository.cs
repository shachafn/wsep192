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
        private static GuestsEntityCollection _guestEntityCollection = new GuestsEntityCollection();

        public void Create(IUser entity)
        {
            _guestEntityCollection.Add(entity.Guid, entity);
        }

        public void Delete(IUser entity)
        {
            _guestEntityCollection.Remove(entity.Guid);
        }

        public void DeleteAll()
        {
            _guestEntityCollection.Clear();
        }

        public void DeleteById(params Guid[] ids)
        {
            if (_guestEntityCollection.ContainsKey(ids.First()))
                _guestEntityCollection.Remove(ids.First());
        }

        public IQueryable<IUser> FindAll()
        {
            return _guestEntityCollection.Values.AsQueryable();
        }

        public IQueryable<IUser> FindByCondition(Expression<Func<IUser, bool>> expression)
        {
            return _guestEntityCollection.AsQueryable().Where(expression);
        }

        public IUser FindById(Guid id)
        {
            if (_guestEntityCollection.ContainsKey(id))
                return _guestEntityCollection[id];
            return null;
        }

        public void Update(IUser entity)
        {
            _guestEntityCollection[entity.Guid] = entity;
        }
    }
}
