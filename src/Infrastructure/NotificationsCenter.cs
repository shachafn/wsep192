using ApplicationCore.Events;
using ApplicationCore.Interfaces.Infastracture;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class NotificationsCenter
    {
        readonly ILogger<NotificationsCenter> _logger;
        readonly IUserNotifier _notifier;

        public NotificationsCenter(ILogger<NotificationsCenter> logger, IUserNotifier notifier)
        {
            _logger = logger;
            _notifier = notifier;
        }

        public async Task HandleUpdate(IUpdateEvent updateEvent)
        {
            //_logger.Log(LogLevel.Information, string.Join('\n', updateEvent.Messages.Values));
            await _notifier.NotifyEvent(updateEvent);
        }

    }
}
