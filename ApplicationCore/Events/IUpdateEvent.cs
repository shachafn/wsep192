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
        ICollection<Guid> Targets { get; }
        string Message { get; }
        void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers);
        void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers);
    }
}
