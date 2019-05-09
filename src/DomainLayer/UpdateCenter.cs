using ApplicationCore.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public static class UpdateCenter
    {
        public delegate Task UpdateEventHandler(IUpdateEvent updateEvent);

        static public event UpdateEventHandler SubscribersHandlers;

        static Dictionary<Guid, Queue<IUpdateEvent>> _persistantEvents = new Dictionary<Guid, Queue<IUpdateEvent>>();

        public static void RaiseEvent(IUpdateEvent updateEvent)
        {
            if (SubscribersHandlers != null)
                SubscribersHandlers(updateEvent);
        }

        public static void Subscribe(UpdateEventHandler eventHandler)
        {
            SubscribersHandlers += eventHandler;
        }
    }
}
