using ApplicationCore.Events;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Infastracture
{
    public interface IUserNotifier
    {
        Task NotifyEvent(IUpdateEvent updateEvent);
    }
}
