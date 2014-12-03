using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Exceptions;
using System;

namespace AlgorithmicTrading.Common.EventSources
{
    public abstract class HistoricalEventSource : EventSource
    {
        protected HistoricalEventSource()
            : base(RunMode.History)
        {
        }

        public Event[] Events { get; protected set; }

        protected override void PreValidate()
        {
            base.PreValidate();

            if (FromTime == DateTime.MaxValue || ToTime == DateTime.MaxValue)
            {
                throw new InvalidEventSouceException("Historical EventSource has invalid FromTime or ToTime.");
            }

            if (ToTime < FromTime)
            {
                throw new InvalidEventSouceException("ToTime of Historical EventSource is less than FromTime.");
            }
        }

        protected override void PostValidate()
        {
        }
    }
}