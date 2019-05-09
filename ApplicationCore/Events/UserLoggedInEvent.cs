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
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }

        public UserLoggedInEvent(Guid initiator)
        {
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
        }

        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Message = $"Welcome {registeredUsers.First(u => u.Guid.Equals(Initiator)).Username}!";
        }
        public void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Targets.Add(Initiator);
        }
    }
}
