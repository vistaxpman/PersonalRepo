using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Strategies;
using System;
using System.Threading;

namespace AlgorithmicTrading.Common.EventEngines
{
    public abstract class EventEngine : IEventEngine
    {
        public RunMode RunMode { get { return Strategy.Context.RunMode; } }

        public IStrategy Strategy { get; set; }

        public abstract DateTime CurrenTime { get; }

        public virtual void Initialize()
        {
            Feedback += (sender, e) => Strategy.ProcessFeedbackEvent((FeedbackEvent)e.Event);
        }

        public abstract void Start();

        public virtual void SendEvent(Event @event)
        {
            OnNewEvent(new EventEventArgs(@event));
        }

        public event EventHandler<EventEventArgs> NewEvent;

        public event EventHandler<EventEventArgs> Feedback;

        protected void OnNewEvent(EventEventArgs e)
        {
            var handler = Interlocked.CompareExchange(ref NewEvent, null, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnFeedback(EventEventArgs e)
        {
            var handler = Interlocked.CompareExchange(ref Feedback, null, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected abstract void PreValidate();

        protected abstract void PostValidate();
    }
}