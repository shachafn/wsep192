using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Events
{
    public interface IUpdateEvent
    {
        Guid Initiator { get; }
        ICollection<Guid> Targets { get; } // Deprecated
        Dictionary<ICollection<Guid>, string> Messages { get; }
        string Message { get; } // Deprecated
        void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers);
        void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers);
        void SetMessages(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers);

    }
}
