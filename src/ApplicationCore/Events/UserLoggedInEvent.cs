using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace ApplicationCore.Events
{
    public class UserLoggedInEvent : IUpdateEvent
    {
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }
        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public UserLoggedInEvent(Guid initiator)
        {
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }

        public void SetMessage(IUnitOfWork unitOfWork)
        {
            Message = $"Welcome {unitOfWork.BaseUserRepository.GetUsername(Initiator)}!";
        }
        public void SetTargets(IUnitOfWork unitOfWork)
        {
            Targets.Add(Initiator);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            string initiatorMsg = $"Welcome {unitOfWork.BaseUserRepository.GetUsername(Initiator)}!";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
        }
    }
}
