using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DomainLayer.Data.Entitites;

namespace DomainLayer.Data.Collections
{
    public class UserEntityCollection : EntityCollection<User>
    {
        public bool ExistsUserWithUsername(string username)
        {
            return _entitiesCollection.Any(user => user.Value.Username.Equals(username));
        }
        public User GetUserByUsername (string username)
        {
            return _entitiesCollection.FirstOrDefault(user => user.Value.Username.Equals(username)).Value;
        }
    }
}
