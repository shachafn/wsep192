using ApplicationCore.Interfaces.DataAccessLayer;
using System;
using System.Collections.Generic;

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
