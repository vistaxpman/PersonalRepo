using AlgorithmicTrading.Common.Contexts;
using AlgorithmicTrading.Common.EventEngines;
using AlgorithmicTrading.Common.Events;
using System;

namespace AlgorithmicTrading.Common.Strategies
{
    public abstract class Strategy : IStrategy
    {
        public virtual string Name { get { return GetType().FullName; } }

        public IContext Context { get; set; }

        public abstract Type[] InputEventTypes { get; }

        public abstract Type[] OutputEventTypes { get; }

        public abstract IEventEngine EventEngine { get; }

        public virtual void Initialize()
        {
            PreValidate();

            EventEngine.Strategy = this;
            EventEngine.Initialize();

            EventEngine.NewEvent += (sender, e) => ((Context)Context).OnEvent(e);
            EventEngine.Feedback += (sender, e) => ((Context)Context).OnEvent(e);

            PostValidate();
        }

        public virtual void Start()
        {
            EventEngine.Start();
        }

        public abstract void ProcessFeedbackEvent(FeedbackEvent @event);

        protected abstract void PreValidate();

        protected abstract void PostValidate();
    }
}