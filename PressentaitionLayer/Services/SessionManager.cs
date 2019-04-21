using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Services
{
    public class SessionManager
    {
        
        public SessionManager(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }
    }
}
