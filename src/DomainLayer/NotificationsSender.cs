using ApplicationCore.Data;
using ApplicationCore.Events;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.Infastracture;
using DomainLayer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    public class NotificationsSender
    {
        readonly ILogger<NotificationsSender> _logger;
        readonly IDomainLayerFacade _domainLayerFacade;
        readonly IUserNotifier _userNotifier;

        public NotificationsSender (ILogger<NotificationsSender> logger, IDomainLayerFacade domainLayerFacade,
            IUserNotifier userNotifier)
        {
            _logger = logger;
            _domainLayerFacade = domainLayerFacade;
            _userNotifier = userNotifier;
        }

        public void HandleUpdate(IUpdateEvent updateEvent)
        {
            var targets = updateEvent.GetTargets(DomainData.ShopsCollection.Values);
            _userNotifier.SendMessageAsync(targets, updateEvent.GetMessage());
        }
    }
}
