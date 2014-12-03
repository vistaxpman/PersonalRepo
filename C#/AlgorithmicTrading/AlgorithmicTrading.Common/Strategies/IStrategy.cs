using AlgorithmicTrading.Common.Contexts;
using AlgorithmicTrading.Common.EventEngines;
using System;
using AlgorithmicTrading.Common.Events;

namespace AlgorithmicTrading.Common.Strategies
{
    public interface IStrategy
    {
        string Name { get; }

        IContext Context { get; set; }

        Type[] InputEventTypes { get; }

        Type[] OutputEventTypes { get; }

        IEventEngine EventEngine { get; }

        void Initialize();

        void Start();

        void ProcessFeedbackEvent(FeedbackEvent @event);
    }
}