using AlgorithmicTrading.Common.Events;
using System;

namespace AlgorithmicTrading.Common
{
    public class EventEventArgs : EventArgs
    {
        public EventEventArgs(Event @event)
        {
            Event = @event;
        }

        public Event Event { get; set; }
    }
}