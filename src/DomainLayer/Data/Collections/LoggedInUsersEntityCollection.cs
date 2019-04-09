using System;
using System.Linq;
using DomainLayer.Data.Entitites;

namespace DomainLayer.Data.Collections
{
    public class LoggedInUsersEntityCollection : EntityCollection<User>
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
