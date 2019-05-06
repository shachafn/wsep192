using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Events
{
    public interface IUpdateEvent
    {
        string GetMessage();
        ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers);
    }
}
