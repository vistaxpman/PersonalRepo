using AlgorithmicTrading.Common.Exceptions;
using System;
using System.Linq;
using System.Threading;

namespace AlgorithmicTrading.Common.EventSources
{
    public abstract class EventSource : IEventSource
    {
        protected EventSource(RunMode runMode)
        {
            RunMode = runMode;
            FromTime = ToTime = DateTime.MaxValue;
        }

        public RunMode RunMode { get; private set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public string[] InstrumentKeys { get; set; }

        public Type[] OutputEventTypes { get; protected set; }

        public virtual void Initialize()
        {
            PreValidate();
            PostValidate();
        }

        public abstract void Start();

        public event EventHandler<EventEventArgs> NewEvent;

        protected void OnNewEvent(EventEventArgs e)
        {
            var handler = Interlocked.CompareExchange(ref NewEvent, null, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void PreValidate()
        {
            if (InstrumentKeys == null)
            {
                throw new InvalidEventSouceException("EventSource has no insturments.");
            }

            if (OutputEventTypes == null || !OutputEventTypes.Any())
            {
                throw new InvalidEventSouceException("EventSource has no OutputEventTypes.");
            }
        }

        protected abstract void PostValidate();
    }
}