using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DAL;
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
        void SetMessage(IUnitOfWork unitOfWork);
        void SetTargets(IUnitOfWork unitOfWork);
        void SetMessages(IUnitOfWork unitOfWork);

    }
}
