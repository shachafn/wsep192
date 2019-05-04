using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer
{
    public interface INotificationClient
    {
        Task RecieveNotification(Guid guid, string notification);
        Task RecieveNotification(string notification);
    }
}
