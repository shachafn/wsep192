using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Services
{
    public interface ISession
    {
        Guid GetSessionGuid { get; }
    }
}
