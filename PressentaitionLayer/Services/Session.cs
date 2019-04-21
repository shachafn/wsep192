using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Services
{
    public class Session : ISession
    {
        public Session() : this(Guid.NewGuid())
        {
        }
        public Session(Guid id)
        {
            SessionGuid = id;
        }

        public Guid SessionGuid { get; private set; }

        public Guid GetSessionGuid => SessionGuid;
    }
}
