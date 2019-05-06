using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace ApplicationCore.Events
{
    public class UserLoggedInEvent : IUpdateEvent
    {
        public Guid UserGuid { get; private set; }

        public UserLoggedInEvent(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        public string GetMessage()
        {
            return "Welcome!";
        }

        public ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            ICollection<Guid> result = new List<Guid>();

            result.Add(UserGuid);

            return result;
        }
    }
}
