using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.EventSources;
using AlgorithmicTrading.Common.Exceptions;
using System;
using System.Linq;

namespace AlgorithmicTrading.Common.Contexts
{
    public class HistoricalContext : Context
    {
        public HistoricalContext()
            : base(RunMode.History)
        {
        }

        public override void Start()
        {
            base.Start();

            // TradingEngine is EventSource but not HistoricalEventSource.
            var events = (from eventSource in EventSources
                          let historicalEventSource = eventSource as HistoricalEventSource
                          where historicalEventSource != null
                          from @event in historicalEventSource.Events
                          select @event).OrderBy(e => e.EventTime);

            var eventEngine = Strategy.EventEngine;
            DateTime preEventTime = DateTime.MinValue;
            foreach (var @event in events)
            {
                if (preEventTime < @event.EventTime)
                {
                    eventEngine.SendEvent(new TimeControllEvent(@event.EventTime));
                    preEventTime = @event.EventTime;
                }
                eventEngine.SendEvent(@event);
            }
        }

        protected override void PreValidate()
        {
            base.PreValidate();

            if (FromTime == DateTime.MaxValue || ToTime == DateTime.MaxValue)
            {
                throw new InvalidContextException("Historical Context has invalid FromTime or ToTime.");
            }

            if (ToTime < FromTime)
            {
                throw new InvalidContextException("ToTime of Historical Context is less than FromTime.");
            }
        }

        protected override void PostValidate()
        {
        }
    }
}