using ApplicationCore.Data;
using ApplicationCore.Events;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.Infastracture;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utils;

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
            await _notifier.NotifyEvent(updateEvent);
        }

    }
}
