using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DAL;

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
            Message = $"Welcome {unitOfWork.UserRepository.FindAll().First(u => u.Guid.Equals(Initiator)).Username}!";
        }
        public void SetTargets(IUnitOfWork unitOfWork)
        {
            Targets.Add(Initiator);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            string initiatorMsg = $"Welcome {unitOfWork.UserRepository.FindAll().First(u => u.Guid.Equals(Initiator)).Username}!";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
        }
    }
}
