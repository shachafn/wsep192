using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace ApplicationCore.Events
{
    public class UserLoggedInEvent : IUpdateEvent
    {
        public Guid UserGuid { get; private set; }
        public string Username { get; private set; }

        public UserLoggedInEvent(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        public string GetMessage()
        {
            return string.Format("Welcome {0} !", Username);
        }

        public ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Username = registeredUsers.First(user => user.Guid.Equals(UserGuid)).Username;
            ICollection<Guid> result = new List<Guid>
            {
                UserGuid
            };
            return result;
        }
    }
}
