using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Infastracture
{
    public interface IUserNotifier
    {
        Task SendMessageAsync(ICollection<Guid> targets, string msg);
        Task SendMessage(Guid target, string msg);
    }
}
